#include <iostream>
#include <string>
#include "asio.hpp"
#include "ConfigParser.h"
#include <filesystem>

using asio::ip::tcp;

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
        std::string hostAddress = config.get("Network", "HostAddress", "127.0.0.1");
        std::string serviceAddress = config.get("Network", "ServiceAddress", "12345");

        asio::io_context io_context;

        tcp::resolver resolver(io_context);
        tcp::resolver::results_type endpoints =
            resolver.resolve(hostAddress, serviceAddress);

        tcp::socket socket(io_context);
        asio::connect(socket, endpoints);

        while (true)
        {
            std::cout << "Enter your message: ";
            std::string request;
            std::getline(std::cin, request);

            if (request.empty())
                break;

            asio::write(socket, asio::buffer(request));

            char reply[1024];
            size_t reply_length = asio::read(socket,
                asio::buffer(reply, request.length()));

            std::cout << "Response from the server: ";
            std::cout.write(reply, reply_length);
            std::cout << std::endl;
        }
    }
    catch (std::exception& e)
    {
        std::cerr << "Error: " << e.what() << std::endl;
    }

    return 0;
}