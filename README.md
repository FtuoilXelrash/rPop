# rPop

![Rust](https://img.shields.io/badge/Game-Rust-orange)
![Umod](https://img.shields.io/badge/Framework-Umod-blue)
![Version](https://img.shields.io/badge/Version-0.0.120-green)
![License](https://img.shields.io/badge/License-MIT-yellow)

A comprehensive Umod plugin for Rust servers that tracks and reports server performance statistics and population data to Discord in real-time.

## 🚀 Features

### 📊 Population Tracking
- **Real-time player counts** (online, sleeping, joining)
- **Admin tracking** - See how many admins are online
- **Population records** - Daily, monthly, and all-time peak players
- **Total unique players** - Track lifetime server players
- **Session analytics** - Average connection time for active players
- **Instant updates** - Discord updates immediately when players join/leave

### 🖥️ Server Performance Monitoring
- **Server FPS** monitoring
- **Memory usage** tracking (used/total)
- **Network I/O** statistics (bytes in/out per second)
- **Server uptime** tracking
- **Map entities** count
- **Server status** (online/offline indicators)

### 🌍 World Information
- **In-game time** display
- **World size** and **seed** information
- **Last wipe date** with time elapsed
- **Blueprint wipe tracking** with formatted dates
- **Server protocol** version display

### 💬 Discord Integration
- **Webhook support** with automatic message editing
- **Rich embeds** with server branding
- **Rate limiting** protection
- **Customizable bot name** and appearance
- **Server header image** support with fallback
- **Thumbnail or full image** options
- **Color-coded status** (green for online, red for offline)

### 🎮 In-Game Commands
- **!pop command** - Display server stats to all players
- **Configurable cooldown** to prevent spam
- **Rich chat formatting** with colors
- **Automatic broadcasting** to all online players

## 📋 Requirements

- **Rust Dedicated Server**
- **Umod (Oxide)** framework
- **Discord webhook** (optional, for Discord integration)

## 🔧 Installation

1. Download the [latest release](https://github.com/FtuoilXelrash/rPop/releases)
2. Copy `rPop.cs` to your server's `oxide/plugins/` directory
3. The plugin will auto-generate its configuration file on first load
4. Configure your Discord webhook (optional) in `oxide/config/rPop.json`
5. Reload the plugin or restart your server

## ⚙️ Configuration

The plugin creates a configuration file at `oxide/config/rPop.json` with extensive customization options:

### Basic Settings
```json
{
  "Settings": {
    "Enable !pop Command": true,
    "Command Cooldown (minutes)": 5.0,
    "Show Last Wipe Date": true,
    "Show Last Blueprint Wipe Date": true,
    "Show Network IO": true,
    "Show Protocol": true,
    "Show Server Status": true
  }
}
```

### Population Display Options
| Option | Default | Description |
|--------|---------|-------------|
| `Show Players Joining` | `true` | Display players in connection queue |
| `Show Players Sleeping` | `true` | Display count of sleeping players |
| `Show Admins Online` | `true` | Display count of online administrators |
| `Hide Zero Values` | `true` | Hide statistics when value is zero |
| `Show Population Records` | `true` | Display daily/monthly/all-time peaks |
| `Show Total Players Ever` | `true` | Show lifetime unique player count |
| `Show Average Connection Time` | `true` | Display average session time |

### Discord Integration
| Option | Default | Description |
|--------|---------|-------------|
| `Discord Webhook URL` | `""` | Your Discord channel webhook URL |
| `Discord Rate Limit (seconds)` | `1.0` | Minimum time between Discord messages |
| `Enable Discord Performance Messages` | `true` | Send automated Discord updates |
| `Performance Message Interval (minutes)` | `3.0` | How often to send regular updates |
| `Discord Bot Name` | `"Live Server Statistics"` | Name displayed in Discord |

### Instant Updates
| Option | Default | Description |
|--------|---------|-------------|
| `Enable Instant Population Updates` | `true` | Send Discord updates when players join/leave |
| `Population Update Delay (seconds)` | `2.0` | Delay before sending instant update |

### Visual Customization
| Option | Default | Description |
|--------|---------|-------------|
| `Use Server Header Image` | `true` | Use server's header image in Discord |
| `Fallback Discord Image URL` | `"https://files.facepunch.com/lewis/1b2911b1/rust-logo.png"` | Image when header unavailable |
| `Use Thumbnail Instead of Image` | `true` | Show image as thumbnail vs full embed |
| `Show Discord Image` | `true` | Display images in Discord embeds |

### In-Game Messages
| Option | Default | Description |
|--------|---------|-------------|
| `Enable In-Game Performance Messages` | `true` | Send periodic in-game broadcasts |
| `In-Game Message Interval (minutes)` | `60.0` | How often to broadcast in-game |

## 🎮 Commands

### Player Commands
| Command | Description | Cooldown |
|---------|-------------|----------|
| `!pop` | Display server statistics to all online players | Configurable (default: 5 minutes) |

### Console Commands
| Command | Description |
|---------|-------------|
| `rpop.help` | Display all available commands and setup information |
| `rpop.test` | Show current statistics and send test Discord message |
| `rpop.performance` | Force immediate Discord performance update |
| `rpop.resetdata` | Reset all population records and statistics |
| `rpop.resetmessage` | Reset Discord message ID (creates new status message) |
| `rpop.status` | Show current server status and timer information |
| `rpop.forceconfig` | Regenerate configuration file with all default options |
| `rpop.testtimer` | Test instant population update and timer reset functionality |

## 📱 Discord Setup

1. **Create a Discord Webhook:**
   - Go to your Discord server settings
   - Navigate to Integrations → Webhooks
   - Click "New Webhook"
   - Select the channel for server updates
   - Copy the webhook URL

2. **Configure the Plugin:**
   ```json
   {
     "Settings": {
       "Discord Webhook URL": "https://discord.com/api/webhooks/YOUR_WEBHOOK_URL_HERE"
     }
   }
   ```

3. **Reload the Plugin:**
   ```
   oxide.reload rPop
   ```

The plugin will automatically create a single Discord message that updates in real-time, rather than spamming your channel with multiple messages.

## 📊 Data Tracking

The plugin stores data in `oxide/data/rPop.json`:

- **Daily peak players** (resets each day)
- **Monthly peak players** (resets each month)
- **All-time peak players** (persistent)
- **Discord message ID** (for message editing)

## 🔄 Update Behavior

### Regular Updates
- Sends Discord updates every 3 minutes (configurable)
- Broadcasts in-game messages every 60 minutes (configurable)

### Instant Updates
- Triggers immediate Discord update when players join/leave
- Configurable delay (default: 2 seconds) to batch rapid connections
- Resets regular update timer after instant updates

### Server Status
- Automatically detects server online/offline status
- Sends final offline update when server shuts down
- Color-codes Discord embeds (green=online, red=offline)

## 🎨 Discord Message Example

```
🤖 Live Server Statistics

✅ Server Status: Online

📊 Population Data
🟢 Players Online: 25/100
🟡 Players In Queue: 2
🔴 Players Sleeping: 15
👑 Admins Online: 3
📈 Today's Peak Players: 45
📊 Monthly Peak Players: 67
🏆 All-Time Peak Players: 89
🏢 Total Server Players: 1,247
⏱️ Average Active Session Time: 2:45:30

🌍 World Data
🕒 In-Game Time: 2:30 PM
🌍 World Size: 4000
🌱 Seed: 1234567890
🏗️ Map Entities: 45,678
🔗 Protocol: 2436.86.0

🖥️ Server Data
💾 Memory Usage: 8,192 MB / 16,384 MB
⚡ Server FPS: 58.3
🌐 Network IO: In: 1.25 KB/s Out: 2.34 KB/s
🕐 Server Online For: 2 days, 14 hours, 32 minutes
🗺️ Last Wipe Date: Jan 15, 2025 (2d ago)
📘 Last Blueprint Wipe Date: Jan 01, 2025 (16d ago)
```

## 🐛 Troubleshooting

### Discord Messages Not Sending
1. Verify webhook URL is correct and active
2. Check console for error messages: `oxide.show debug`
3. Test with: `rpop.test`
4. Ensure rate limiting isn't blocking messages

### Performance Issues
1. Increase update intervals in configuration
2. Disable instant updates if causing lag
3. Check server FPS with `rpop.status`

### Configuration Problems
1. Regenerate config: `rpop.forceconfig`
2. Check JSON syntax with online validators
3. Reload plugin: `oxide.reload rPop`

## 📈 Performance Impact

- **Minimal CPU usage** - Efficient update timers
- **Low memory footprint** - Lightweight data structures  
- **Rate limiting** - Prevents Discord API abuse
- **Configurable intervals** - Adjust for server performance

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly on a development server
5. Submit a pull request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author
[Ftuoil Xelrash](https://github.com/FtuoilXelrash)



## 🔗 Links

- [Download Latest Release](https://github.com/FtuoilXelrash/rPop/releases)
- [Report Issues](https://github.com/FtuoilXelrash/rPop/issues)


---

⭐ **Star this repository if you find it useful!** ⭐