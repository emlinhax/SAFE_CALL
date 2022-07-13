# SAFE_CALL

This simple library allows you to protect yourself against reverse engineers.
It works on x86 and x64 and also allows calls to .NET Framework functions as seen in the usage-example below.

Usage:
``` cs
_.SAFE_CALL(typeof(Console), "WriteLine", "Hello World!");
```

NOTE: It works best with static methods and doesnt really like instances. That will hopefully change in the future.


Internals of what the library does:
1. Check if the function you are trying to call is hooked
2. Check if IsDebuggerPresent is hooked and also check what it returns.
3. Create a DynamicMethod and copy over the il code generated by the disassembler (shadow-copy).
4. Invoke the new shadow-copy and return its output.

#### ShadowCopy:
Creating a shadow-copy will prevent reversers from 
placing a breakpoint on the function you are trying to call 
and makes it hard to follow the program-flow
