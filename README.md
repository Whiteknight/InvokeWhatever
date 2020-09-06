# InvokeWhatever

It's rare but it happens: You have a bunch of values, and you need to invoke a method that may or may not take some of those values as arguments, in whatever order. Maybe you're trying to interact with external code that's changing too often. Or maybe you're interacting with generated code. Or maybe you have a bunch of existing scripts that are all written slightly differently, but you want to try an "unify" them together into a single application. Whatever your use-case, just invoke the darn thing.

## Usage

```csharp
var result = Invoke.InstanceMethod(object, "MethodName", ...available arguments...);
var result = Invoke.StaticMethod(type, "MethodName", ...available arguments...);
var result = Invoke.Constructor(type, ...available arguments...);
```

**InvokeWhatever** tries to invoke a method or constructor, given a collection of arguments. If there are multiple constructors, or multiple method overrides with the same name, InvokeWhatever attempts to find the best match. The "best match" is based on a simple scoring system, that attempts to use as many of the values you have provided as possible. More arguments that match more closely produces a better score. The best available method is invoked, and the result (if any) is returned.

## FAQ

### Like, why?

Every once in a while I run into some weird situation where I have to invoke a method in code I do not control, and which I cannot guarantee conforms to any particular interface or contract. I'm tired of solving this same stupid problem every time it pops up, so I'm publishing a single stupid solution here so I never have to think about it again.

### Isn't this like a DI container?

DI/IoC containers do construct objects using an unordered collection of objects. But containers do so much more, such as controlling lifetimes and optimizing. *InvokeWhatever* doesn't do any of that fancy stuff. It's a very small, lightweight solution to a very rare, stupid problem. If you are using a DI/IoC container and would like to leverage that for your situation, that's probably a better idea. 

### How does it work?

InvokeWhatever uses a very simple algorithm: For each parameter in a method or constructor, it loops through the available argument types and tries to find an argument which matches. Each argument->parameter match adds to a running score for the method, with better matches making better scores. Exact type matches are scored highest, followed by super-type matches, matches where a value type needs to get boxed as a reference type, matches where the parameter is type `object`, and finally a situation where no arguments match but the parameter has a default value which can be used. The worst possible match is for a parameterless constructor/method, which will be used as the last resort if no other overrides can be matched properly.

Arguments are examined linearly. If you pass multiple objects of the same type, and the method has multiple parameters of that type, the arguments will be consumed in order. No argument will be passed more than once, so if you want duplicates, you need to pass duplicates.

### Can I specify which Constructor/Method override to use?

If you already know the number of parameters and their types, just use a regular method/constructor call and don't use this library. If you have all that information, you're already in a better place than I was when I had to write this library.

### Can I customize behavior?

You can use create a custom `MethodSortOptions` object to adjust some of the weights used for different matching situations. Beyond that, the matching/sorting/invoking code really isn't going to change much. Considering how rare it is to run into a situation where this library is necessary, it's not really worth a lot of effort to improve it.

### Can I specify types to pass `null` for?

Not at the moment, no. *InvokeWhatever* doesn't want to become a DI container, and it doesn't want to be keeping track of objects, type information, and other metadata. You can pass a `null` value in your list of available arguments, and *InvokeWhatever* will treat it as an `object` and only use that `null` value for parameters typed as `object`. If we allowed a reference type to be assigned the value of `null`, then any override might end up matching and things will get all out of whack.
