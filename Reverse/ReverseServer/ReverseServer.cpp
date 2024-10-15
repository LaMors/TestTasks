#include <iostream>
#include <string>
#include <memory>
#include "asio.hpp"
#include "ConfigParser.h"
#include <filesystem>

using asio::ip::tcp;

class session : public std::enable_shared_from_this<session>
{
public:
    session(tcp::socket socket)
        : socket_(std::move(socket))
    {
    }

    void start()
    {
        do_read();
    }

private:
    void do_read()
    {
        auto self(shared_from_this());
        socket_.async_read_some(asio::buffer(data_, max_length),
            [this, self](std::error_code ec, std::size_t length)
            {
                if (!ec)
                {
                    std::reverse(data_, data_ + length);
                    do_write(length);
                }
            });
    }

    void do_write(std::size_t length)
    {
        auto self(shared_from_this());
        asio::async_write(socket_, asio::buffer(data_, length),
            [this, self](std::error_code ec, std::size_t)
            {
                if (!ec)
                {
                    do_read();
                }
            });
    }

    tcp::socket socket_;
    enum { max_length = 1024 };
    char data_[max_length];
};

class server
{
public:
    server(asio::io_context& io_context, short port)
        : acceptor_(io_context, tcp::endpoint(tcp::v4(), port))
    {
        do_accept();
    }

private:
    void do_accept()
    {
        acceptor_.async_accept(
            [this](std::error_code ec, tcp::socket socket)
            {
                if (!ec)
                {
                    std::make_shared<session>(std::move(socket))->start();
                }

                do_accept();
            });
    }

    tcp::acceptor acceptor_;
};

int main()
{
    try
    {
        ConfigParser config;

        std::filesystem::path configPath = SOLUTION_DIR;
        configPath /= "config.ini";

        if (!config.load(configPath.string()))
        {
            std::cerr << "Failed to load config.ini from path: " << configPath << std::endl;
            return 1;
        }

        std::string serviceAddress = config.get("Network", "ServiceAddress", "12345");
        unsigned short port = static_cast<unsigned short>(std::stoi(serviceAddress));

        asio::io_context io_context;

        server s(io_context, port);

        std::cout << "The server is running on port " << port << "..." << std::endl;

        io_context.run();
    }
    catch (std::exception& e)
    {
        std::cerr << "Error: " << e.what() << std::endl;
    }

    return 0;
}