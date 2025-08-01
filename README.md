# rPop

![Rust](https://img.shields.io/badge/Game-Rust-orange)
![Umod](https://img.shields.io/badge/Framework-Umod-blue)
![Version](https://img.shields.io/badge/Version-0.0.133-green)
![License](https://img.shields.io/badge/License-MIT-yellow)

A comprehensive Umod plugin for Rust servers that tracks and reports server performance statistics and population data to Discord in real-time with automatic message editing and instant population updates.

## 🚀 Features

### 📊 Population Tracking
- **Real-time player counts** (online, sleeping, joining)
- **Admin tracking** - See how many admins are online
- **Population records** - Daily, monthly, and all-time peak players with timestamps
- **Total unique players** - Track lifetime server players from userdata folders
- **Session analytics** - Average connection time for currently active players
- **Instant updates** - Discord updates immediately when players join/leave
- **Smart data management** - Automatic daily and monthly record resets

### 🖥️ Server Performance Monitoring
- **Server FPS** monitoring with real-time updates
- **Memory usage** tracking (used/total system memory)
- **Network I/O** statistics (bytes in/out per second)
- **Server uptime** tracking with human-readable format
- **Map entities** count for performance optimization
- **Server status** (online/offline indicators with emoji support)

### 🌍 World Information
- **In-game time** display with 12-hour format
- **World size** and **seed** information
- **Last wipe date** with intelligent time elapsed formatting
- **Blueprint wipe tracking** with separate date tracking
- **Next wipe prediction** - Automatic first Thursday calculation with timezone support
- **Server protocol** version display (network.save.report format)
- **Timezone awareness** - Automatic server timezone detection and display

### 💬 Advanced Discord Integration
- **Smart webhook management** with message editing (no spam!)
- **Rich embeds** with server branding and custom images
- **Rate limiting** protection with configurable intervals
- **Customizable bot name** and avatar
- **Server header image** support with intelligent fallback
- **Thumbnail or full image** options
- **Color-coded status** (cyan for online, red for offline)
- **Persistent message tracking** - Single message that updates continuously
- **Error handling** - Automatic recovery from deleted messages

### 🎮 Enhanced In-Game Features
- **!pop command** - Display comprehensive server stats to all players
- **Configurable cooldown** with intelligent time display
- **Rich chat formatting** with colors and proper spacing
- **Automatic broadcasting** to all online players
- **In-game performance messages** - Periodic status updates
- **Zero value hiding** - Cleaner display when values are zero

### ⚡ Smart Update System
- **Instant population updates** - Immediate Discord updates on player events
- **Configurable delays** - Batch rapid connections to prevent spam
- **Timer management** - Intelligent timer reset after instant updates
- **Performance optimization** - Minimal server impact with efficient scheduling

## 📋 Requirements

- **Rust Dedicated Server**
- **Umod (Oxide)** framework
- **Discord webhook** (optional, for Discord integration)
- **Valid Steam userdata** (for total player tracking)

## 🔧 Installation

1. Download the [latest release](https://github.com/FtuoilXelrash/rPop/releases)
2. Copy `rPop.cs` to your server's `oxide/plugins/` directory
3. The plugin will auto-generate its configuration file on first load
4. Configure your Discord webhook (optional) in `oxide/config/rPop.json`
5. Reload the plugin or restart your server

## ⚙️ Complete Configuration Reference

The plugin creates a comprehensive configuration file at `oxide/config/rPop.json`:

### Core Settings
```json
{
  "Settings": {
    "Enable !pop Command": true,
    "Command Cooldown (minutes)": 5.0,
    "Show Last Wipe Date": true,
    "Show Last Blueprint Wipe Date": true,
    "Show Next Wipe Date": true,
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
| `Hide Zero Values` | `true` | Hide statistics when value is zero (cleaner display) |
| `Show Population Records` | `true` | Display daily/monthly/all-time peaks with dates |
| `Show Total Players Ever` | `true` | Show lifetime unique player count from userdata |
| `Show Average Connection Time` | `true` | Display average session time for active players |

### Discord Integration Settings
| Option | Default | Description |
|--------|---------|-------------|
| `Discord Webhook URL` | `""` | Your Discord channel webhook URL |
| `Discord Rate Limit (seconds)` | `1.0` | Minimum time between Discord API calls |
| `Enable Discord Performance Messages` | `true` | Send automated Discord updates |
| `Performance Message Interval (minutes)` | `3.0` | Regular update frequency |
| `Discord Bot Name` | `"Live Server Statistics"` | Bot name displayed in Discord |

### Instant Update System
| Option | Default | Description |
|--------|---------|-------------|
| `Enable Instant Population Updates` | `true` | Send immediate Discord updates on player events |
| `Population Update Delay (seconds)` | `2.0` | Delay to batch rapid player connections |

### Visual Customization
| Option | Default | Description |
|--------|---------|-------------|
| `Use Server Header Image` | `true` | Use server's header image in Discord embeds |
| `Fallback Discord Image URL` | `"https://files.facepunch.com/lewis/1b2911b1/rust-logo.png"` | Fallback when header unavailable |
| `Use Thumbnail Instead of Image` | `true` | Display as thumbnail vs full embed image |
| `Show Discord Image` | `true` | Enable/disable images in Discord embeds |

### In-Game Broadcasting
| Option | Default | Description |
|--------|---------|-------------|
| `Enable In-Game Performance Messages` | `true` | Send periodic in-game status broadcasts |
| `In-Game Message Interval (minutes)` | `60.0` | Frequency of in-game announcements |

### Information Display Controls
| Option | Default | Description |
|--------|---------|-------------|
| `Show Last Wipe Date` | `true` | Display last server wipe with time elapsed |
| `Show Last Blueprint Wipe Date` | `true` | Show last blueprint wipe date |
| `Show Next Wipe Date` | `true` | Show predicted next wipe (first Thursday) |
| `Show Network IO` | `true` | Display network traffic statistics |
| `Show Protocol` | `true` | Show server protocol version |
| `Show Server Status` | `true` | Display online/offline status with emoji |

## 🎮 Commands

### Player Commands
| Command | Description | Cooldown | Access |
|---------|-------------|----------|--------|
| `!pop` | Display comprehensive server statistics to all online players | Configurable (default: 5 minutes) | All players |

### Console Commands
| Command | Description | Access |
|---------|-------------|--------|
| `rpop.help` | Display comprehensive help with setup information | Admin |
| `rpop.test` | Show current statistics and send test Discord message | Admin |
| `rpop.performance` | Force immediate Discord performance update | Admin |
| `rpop.resetdata` | Reset all population records and statistics | Admin |
| `rpop.resetmessage` | Reset Discord message ID (creates new status message) | Admin |
| `rpop.status` | Show detailed server status and timer information | Admin |
| `rpop.forceconfig` | Regenerate configuration file with all default options | Admin |

## 📱 Discord Setup Guide

### Step 1: Create Discord Webhook
1. **Access Server Settings:**
   - Right-click your Discord server
   - Select "Server Settings"
   - Navigate to "Integrations"

2. **Create Webhook:**
   - Click "Webhooks" → "New Webhook"
   - Name it (e.g., "Rust Server Stats")
   - Select the channel for updates
   - Copy the webhook URL

3. **Security Note:**
   - Keep your webhook URL private
   - Don't share it publicly or commit it to version control

### Step 2: Configure Plugin
```json
{
  "Settings": {
    "Discord Webhook URL": "https://discord.com/api/webhooks/YOUR_WEBHOOK_ID/YOUR_WEBHOOK_TOKEN",
    "Discord Bot Name": "Your Server Name - Live Stats",
    "Performance Message Interval (minutes)": 3.0
  }
}
```

### Step 3: Test Setup
```bash
# In server console
rpop.test
```

The plugin automatically creates a **single Discord message** that updates continuously, preventing channel spam.

## 📊 Data Management

### Automatic Data Files
The plugin manages data in `oxide/data/rPop.json`:

```json
{
  "Today High Population": {
    "Count": 45,
    "Date": "2025-01-15T14:30:00"
  },
  "Monthly High Population": {
    "Count": 67,
    "Date": "2025-01-10T16:45:00"
  },
  "All Time High Population": {
    "Count": 89,
    "Date": "2024-12-25T20:15:00"
  },
  "Last Reset Date": "2025-01-15T00:00:00",
  "Last Monthly Reset": "2025-01-01T00:00:00",
  "Discord Status Message ID": "1234567890123456789"
}
```

### Smart Reset System
- **Daily Reset:** Population records reset at midnight server time
- **Monthly Reset:** Monthly records reset on the 1st of each month
- **Persistent Storage:** All-time records and total player counts are preserved

### Player Tracking
- **Total Players:** Counted from valid Steam ID folders in `userdata/`
- **Session Times:** Calculated from active connection duration
- **Steam ID Validation:** Ensures accurate player counting with proper format validation

## 🔄 Update Behavior & Performance

### Regular Update Cycle
- **Discord Updates:** Every 3 minutes (configurable)
- **In-Game Messages:** Every 60 minutes (configurable)
- **Population Records:** Updated on every player join/leave

### Instant Update System
- **Trigger:** Player connections/disconnections
- **Smart Batching:** 2-second delay to group rapid events
- **Timer Reset:** Regular update timer resets after instant updates
- **Rate Limiting:** Prevents Discord API abuse

### Server Status Detection
- **Online Detection:** Automatic during `OnServerInitialized`
- **Offline Detection:** Triggered during `OnServerShutdown`
- **Final Update:** Sends offline status before shutdown
- **Visual Indicators:** Color-coded Discord embeds and emoji status

### Performance Optimization
- **Minimal CPU Usage:** Efficient timer management
- **Memory Efficient:** Lightweight data structures
- **Error Handling:** Robust exception management
- **Validation:** Configuration validation with auto-repair

## 🎨 Discord Message Example

```
[Your Server Name]

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
📅 Next Wipe: Feb 06, 2025 1:00 PM CST (in 21d 18h 30m)
📘 Last BP Wipe: Jan 01, 2025 (16d ago)

rPop Live Server Statistics V1.0.0 by Ftuoil Xelrash
```

## 🕐 Wipe Schedule System

### Automatic Wipe Prediction
- **Schedule:** First Thursday of each month at 1:00 PM server time
- **Timezone Detection:** Automatic server timezone identification
- **Smart Formatting:** Intelligent time remaining display
- **Live Updates:** Countdown updates in real-time

### Supported Timezones
- **CST/CDT** - Central Time
- **EST/EDT** - Eastern Time  
- **MST/MDT** - Mountain Time
- **PST/PDT** - Pacific Time
- **UTC/GMT** - Universal Time
- **Fallback** - "Server Time" for unknown zones

### Wipe Date Examples
```
Today 1:00 PM CST (Wipe in progress!)
Tomorrow 1:00 PM EST (in 18h 30m)
Feb 06, 2025 1:00 PM PST (in 21d 18h 30m)
```

## 🐛 Troubleshooting

### Discord Integration Issues

**Messages Not Sending:**
```bash
# Check webhook URL
rpop.test

# Verify console output
oxide.show debug

# Reset message tracking
rpop.resetmessage
```

**Rate Limiting:**
- Increase `Discord Rate Limit (seconds)` in config
- Check for multiple plugins using same webhook
- Monitor console for rate limit warnings

**Message Editing Fails:**
```bash
# Reset Discord message ID
rpop.resetmessage

# Force new message creation
rpop.performance
```

### Performance Issues

**High CPU Usage:**
- Increase update intervals in configuration
- Disable instant updates temporarily
- Check for conflicting plugins

**Memory Problems:**
```bash
# Check current memory usage
rpop.status

# Reset data if corrupted
rpop.resetdata
```

**FPS Drops:**
- Increase `Performance Message Interval`
- Disable `Enable In-Game Performance Messages`
- Monitor with `rpop.status`

### Configuration Problems

**Missing Options:**
```bash
# Auto-repair configuration
oxide.reload rPop

# Force complete regeneration
rpop.forceconfig
```

**Invalid JSON:**
- Use online JSON validator
- Check for trailing commas
- Verify quotation marks

**Reset to Defaults:**
```bash
rpop.forceconfig
oxide.reload rPop
```

### Data Tracking Issues

**Incorrect Player Count:**
- Verify `userdata/` folder permissions
- Check Steam ID validation
- Use `rpop.test` to verify counting

**Population Records Not Updating:**
```bash
# Force manual update
rpop.performance

# Reset all records
rpop.resetdata
```

**Session Time Errors:**
- Restart plugin: `oxide.reload rPop`
- Check for player connection issues
- Verify network stability

## 📈 Performance Impact

### Resource Usage
- **CPU Impact:** < 1% on modern hardware
- **Memory Footprint:** ~2-5 MB RAM usage
- **Network Overhead:** Minimal Discord API calls
- **Disk I/O:** Lightweight JSON file operations

### Optimization Features
- **Smart Caching:** Efficient data structure reuse
- **Rate Limiting:** Prevents API abuse and server strain
- **Batch Updates:** Groups rapid events to reduce overhead
- **Error Recovery:** Graceful handling of network issues

### Recommended Settings
For **High Population Servers** (100+ players):
```json
{
  "Performance Message Interval (minutes)": 5.0,
  "Population Update Delay (seconds)": 3.0,
  "Discord Rate Limit (seconds)": 2.0
}
```

For **Low Population Servers** (< 50 players):
```json
{
  "Performance Message Interval (minutes)": 2.0,
  "Population Update Delay (seconds)": 1.0,
  "Discord Rate Limit (seconds)": 0.5
}
```

## 🔧 Advanced Configuration

### Custom Wipe Schedule
If your server doesn't follow the standard first Thursday schedule, you can modify the wipe prediction logic or disable next wipe display:

```json
{
  "Show Next Wipe Date": false
}
```

### Image Customization
```json
{
  "Use Server Header Image": true,
  "Fallback Discord Image URL": "https://your-custom-image.com/image.png",
  "Use Thumbnail Instead of Image": false,
  "Show Discord Image": true
}
```

### Minimal Discord Setup
For bandwidth-conscious setups:
```json
{
  "Show Discord Image": false,
  "Performance Message Interval (minutes)": 10.0,
  "Enable Instant Population Updates": false
}
```

## 🤝 Contributing

We welcome contributions! Here's how to get started:

1. **Fork the Repository**
   ```bash
   git clone https://github.com/YourUsername/rPop.git
   ```

2. **Create Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Follow Coding Standards**
   - Use [Umod Approval Guidelines](https://umod.org/documentation/api/approval-guide)
   - Add comprehensive error handling
   - Include configuration validation
   - Test on development server

4. **Submit Pull Request**
   - Include detailed description
   - Test all functionality
   - Update documentation if needed

### Development Setup
```bash
# Development server testing
oxide.reload rPop
rpop.test
rpop.status
```

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Ftuoil Xelrash**
- GitHub: [@FtuoilXelrash](https://github.com/FtuoilXelrash)
- **[Discord Plugin Support](https://discord.gg/G8mfZH2TMp)** - Author Community support

## 🆘 Support

### Getting Help
1. **Check Documentation:** Review this README thoroughly
2. **Console Commands:** Use `rpop.help` for quick reference
3. **Test Setup:** Run `rpop.test` to verify functionality
4. **Issue Reports:** Use GitHub Issues with detailed information

### Issue Template
When reporting bugs, please include:
```
**Plugin Version:** 1.0.0
**Umod Version:** [Your Version]
**Server Population:** [Typical player count]
**Error Message:** [Full console output]
**Configuration:** [Relevant config settings]
**Steps to Reproduce:** [Detailed steps]
```

## 🔗 Links

- **[Download Latest Release](https://github.com/FtuoilXelrash/rPop/releases)** - Always get the newest version
- **[Report Issues](https://github.com/FtuoilXelrash/rPop/issues)** - Bug reports and feature requests


## 🏆 Recognition

Special thanks to:
- **Umod Team** - For the excellent modding framework
- **Rust Community** - For feedback and testing
- **Contributors** - For improvements and bug reports

---

## 📊 Statistics

![GitHub Downloads](https://img.shields.io/github/downloads/FtuoilXelrash/rPop/total)
![GitHub Stars](https://img.shields.io/github/stars/FtuoilXelrash/rPop)
![GitHub Issues](https://img.shields.io/github/issues/FtuoilXelrash/rPop)
![GitHub License](https://img.shields.io/github/license/FtuoilXelrash/rPop)

⭐ **Star this repository if you find it useful!** ⭐
