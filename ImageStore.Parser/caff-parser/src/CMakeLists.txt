add_executable(caff-parser "")
target_compile_options(caff-parser
    PRIVATE 
    $<$<CONFIG:Debug>: -O0 -g>
    $<$<CONFIG:Release>: -O2>
    -Wall -pedantic
    )
target_compile_features(caff-parser PUBLIC cxx_std_17)
target_include_directories(caff-parser
    PUBLIC
    ${CAFF_PARSER_SOURCE_DIR}/include
    ${CAFF_PARSER_SOURCE_DIR}/third_party/lodepng
    )
target_sources(caff-parser
    PRIVATE
    caff.cpp
    caff_parser.cpp
    ciff.cpp
    PUBLIC
    ${CAFF_PARSER_SOURCE_DIR}/include/caff.hpp
    ${CAFF_PARSER_SOURCE_DIR}/include/ciff.hpp
    ${CAFF_PARSER_SOURCE_DIR}/include/parsing_error.hpp
    ${CAFF_PARSER_SOURCE_DIR}/include/util.hpp
    )
target_link_libraries(caff-parser
    PUBLIC
    lodepng)
