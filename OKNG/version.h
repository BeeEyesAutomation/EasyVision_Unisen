#define VER_MAJOR 1
#define VER_MINOR 0
#define VER_BUILD 7
#define VER_REV   0

#define STR_HELPER(x) #x
#define STR(x) STR_HELPER(x)
#define FILEVER_STR STR(VER_MAJOR) "." STR(VER_MINOR) "." STR(VER_BUILD) "." STR(VER_REV)
