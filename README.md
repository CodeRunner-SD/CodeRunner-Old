# CodeRunner

[![](https://img.shields.io/github/stars/StardustDL/CodeRunner.svg?style=social&label=Stars)](https://github.com/StardustDL/CodeRunner) [![](https://img.shields.io/github/forks/StardustDL/CodeRunner.svg?style=social&label=Fork)](https://github.com/StardustDL/CodeRunner) ![](http://progressed.io/bar/40?title=developing) [![](https://img.shields.io/github/license/StardustDL/CodeRunner.svg)](https://github.com/StardustDL/CodeRunner/blob/master/LICENSE)

A CLI tool to run code. Inspired by [edl-cr](https://github.com/eXceediDeaL/edl-coderunner).

Project Status:

|||
|-|-|
|Repository|[![issue](https://img.shields.io/github/issues/StardustDL/CodeRunner.svg)](https://github.com/StardustDL/CodeRunner/issues/) [![pull requests](https://img.shields.io/github/issues-pr/StardustDL/CodeRunner.svg)](https://github.com/StardustDL/CodeRunner/pulls/)|
|Dependencies|[![dependencies](https://img.shields.io/librariesio/github/StardustDL/CodeRunner.svg)](https://libraries.io/github/StardustDL/CodeRunner)|
|Build|[![master](https://img.shields.io/travis/StardustDL/CodeRunner/master.svg?label=master)](https://travis-ci.org/StardustDL/CodeRunner) [![dev](https://img.shields.io/travis/StardustDL/CodeRunner/dev.svg?label=dev)](https://travis-ci.org/StardustDL/CodeRunner)|
|Coverage|[![master](https://img.shields.io/codecov/c/github/StardustDL/CodeRunner/master.svg?label=master)](https://codecov.io/gh/StardustDL/CodeRunner) [![dev](https://img.shields.io/codecov/c/github/StardustDL/CodeRunner/dev.svg?label=dev)](https://codecov.io/gh/StardustDL/CodeRunner)|

## CLI Mode

|Option|Description|
|-|-|
|`-d --dir`|Set working directory|
|`-c --command`|Execute command just like in interactive mode|
|`-V --version`|Show CR version|
|`-v --verbose`|Enable `DEBUG` level for logging|

## Interactive Mode

If you don't use `--command` options, CR will run in interactive mode.

### Initialize

```sh
> init
```

Initialize CR data. It will create a directory named `.cr` in current directory.

If you want to clear CR data, use this command:

```sh
> init --delete
```

### Create and Run

Create a new item:

```sh
> new cpp -- name=a

# input name interactively
> new cpp
```

It will use templates in `.cr/templates/` to create item.

If you want to set current work-item with an existed file, use this:

```sh
> now -f a.cpp
```

Then use `run` command to run code.

```sh
# run a.cpp
a.cpp> run cpp
```

### Use Directory

Not only use files, you can also use directories to create a unique environment for codes.

```sh
# Create a new directory env
> new dir -- name=a

# Set a directory env for current
> now -d a

# Run
@a> run dir
```

For `run` command, it will use the command list in `settings.json` in the directory. You can write your own commands in it. And these command works in the directory of current work-item.

### Debug

When you meet some errors, for example, CR data loading failing, use `debug` command to get some information. This is also a useful tool when you create an issue.

### Commands

|Command|Description|
|-|-|
|`init [--delete]`|Initialize or delete CR data|
|`clear`|Clear screen|
|`new [Template]`|Create new item by template|
|`now -f [file] -d [directory]`|Change current work-item|
|`run [Operation]`|Run operation|
|`template [list add remove]`|Manage templates|
|`operation [list add remove]`|Manage operations|
|`debug`|Show debug data|
|`quit`|Quit CR|
|`--help`|Get help|
|`--version`|Get version|

# Config

The config files is at `.cr/`

## templates/

This directory contains templates.

## operations/

This directory contains operations.
