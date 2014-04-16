LibAnimaonline
==============

LibAnimaonline

A set of useful cross platform helper classes to use with .NET, written in C#

Project Structure
-----------
* [Events\SmartEvent.cs] - An easy to use event handler, supports subscribing / unsubscribing and async / synchronous execution
* [Extensions] - A set of useful extension methods for various .NET types
* [ILTools] - A virtual CLR simulator (Just for fun) and a set of extensions for decoding IL bytes GetType().GetMethod().GetInstructions()
* [Reflection\ChangeWatcher.cs] - Detect changed properties of an object
* [Reflection\TypeExplorer.cs] - List all loaded namespaces, types, etc.
* [Threading\BlockingContext.cs] - Blocks the executing thread (similar to AutoResetEvent) while awaiting for tasks
