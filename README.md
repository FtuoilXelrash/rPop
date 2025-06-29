# rPop
Rust game server extension to provide **JSON Web API**.
It will made possible to grant access with permissions to particular plugins methods or to execute any hooks via web request.

With this **Web API** anyone can create own external applications (web, mobile, etc.) and use server plugins to retrieve/send any information from/to game server.

Why not use RCON? Cause it's easier to call and RustApi can provide limited access to server (RCON provide only full access).

# Status
Testing and improvement.

# How to use extension
1. Download [latest version](https://github.com/FtuoilXelrash/rPop/releases)
2. Copy `rPop.cs` file to `\Rust\oxide\plugins\rPop.cs`


After plugin has been loaded, you can find new configuration file here:
`\Rust\oxide\config\rPop.json`

## Configuration
.

[Read more](rPop.md) about configuration file.

.

[Read more](Commands.md).

# Console commands
- `api.help` - list of available console api commands
- `api.reload` - Reload extenstion configuration from file
- `api.reload_users` - Reload extenstion users from file
- `api.version` - Installed version of RustApi extension
- `api.commands` - Cached API commands

# Players commands
!pop
