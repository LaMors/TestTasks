#include "pch.h"
#include "ConfigParser.h"
#include <fstream>
#include <sstream>

bool ConfigParser::load(const std::string& filename)
{
    std::ifstream file(filename);
    if (!file)
        return false;

    std::string line;
    std::string currentSection;

    while (std::getline(file, line))
    {
        line = trim(line);

        if (line.empty() || line[0] == ';' || line[0] == '#')
            continue;

        if (line.front() == '[' && line.back() == ']')
        {
            currentSection = line.substr(1, line.size() - 2);
            continue;
        }

        auto pos = line.find('=');
        if (pos == std::string::npos)
            continue;

        std::string key = trim(line.substr(0, pos));
        std::string value = trim(line.substr(pos + 1));

        data_[currentSection + "." + key] = value;
    }

    return true;
}

std::string ConfigParser::get(const std::string& section, const std::string& key, const std::string& defaultValue) const
{
    auto it = data_.find(section + "." + key);
    if (it != data_.end())
        return it->second;
    else
        return defaultValue;
}

std::string ConfigParser::trim(const std::string& s)
{
    const char* whitespace = " \t\n\r\f\v";
    size_t start = s.find_first_not_of(whitespace);
    if (start == std::string::npos)
        return "";
    size_t end = s.find_last_not_of(whitespace);
    return s.substr(start, end - start + 1);
}