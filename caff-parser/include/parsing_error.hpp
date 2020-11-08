#ifndef PARSING_ERROR_HPP
#define PARSING_ERROR_HPP

#include <stdexcept>

namespace cp {
class ParsingError : public std::runtime_error {
public:
    inline ParsingError(const char *str);
};

ParsingError::ParsingError(const char *str) : std::runtime_error(str)
{
}
} // namespace cp

#endif
