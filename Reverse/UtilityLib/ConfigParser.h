#pragma once
#include <string>
#include <unordered_map>

class ConfigParser
{
public:
    bool load(const std::string& filename);
    std::string get(const std::string& section, const std::string& key, const std::string& defaultValue = "") const;

private:
    static std::string trim(const std::string& s);
    std::unordered_map<std::string, std::string> data_;
};