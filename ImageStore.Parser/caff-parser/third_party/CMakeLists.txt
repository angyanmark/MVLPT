add_library(lodepng STATIC "")
target_compile_options(lodepng
    PRIVATE 
    $<$<CONFIG:Debug>: -O0 -g>
    $<$<CONFIG:Release>: -O2>
    -Wall -pedantic
    )
target_compile_features(lodepng PUBLIC cxx_std_17)
target_sources(lodepng
    PRIVATE
    lodepng/lodepng.cpp
    PUBLIC
    ${CAFF_PARSER_SOURCE_DIR}/third_party/lodepng/lodepng.h
    )
