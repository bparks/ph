ph - package headers
====================

This is a small tool to package mutiple header files included from a single
"entry point" header into one larger header file suitable for copying around.

Basically, it does the following:
1. Scans the specified file for all `#include` directives.
2. Leaves those it finds in the system include directories as-is
3. Includes those in the same source folder as contains the entry point
4. Only includes headers the *first* time they are encountered to avoid
   duplication (note that this may have side effects if header files are
   used counter to convention and actually intended to be duplicated)

Some things on the horizon:
* Be able to specify "exclude" directories (directories searched for
  includes but not included in the final bundle)
* Be able to specify "include" directories (directories searched for
  files which *should* be included in the final bundle)
* Be able to specify multiple entry points
* Be able to specify an output file (currently writes to std out)
* Be able to exclude a specific file from the bundle
* Add line number debugging information

Building and running
--------------------

This is a .NET Core app (see FAQ, Why is `ph` a .NET Core app?), so as
long as you have the .NET Core SDK installed, just do

    dotnet run --project ph.csproj <path to entry point>


FAQ
---

**Why is `ph` a .NET Core app?**

Initially, I started building `ph` in C++, but sadly my comfort level with
regular expressions in C++ is lacking far behind my comfort level with them
in C#. It turned out that I could get `ph` to a working state in just a couple
hours with C#, while it would have taken significantly longer in C++.

In the future, I could definitely see `ph` being rewritten in C++ or Rust or
Go (or whatever is cool at the time) to make it even more portable. But for
now, writing it as a .NET Core app suits my purposes.
