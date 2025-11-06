===============================================================================
                            rPop - Live Server Statistics
                                 by Ftuoil Xelrash
===============================================================================

A comprehensive Umod plugin for Rust servers that tracks and reports server 
performance statistics and population data to Discord in real-time with 
automatic message editing and instant population updates.

===============================================================================
                                   FEATURES
===============================================================================

POPULATION TRACKING:
- Real-time player counts (online, sleeping, joining)
- Admin tracking - See how many admins are online
- Population records - Daily, monthly, and all-time peak players
- Total unique players - Track lifetime server players
- Session analytics - Average connection time for active players
- Instant updates - Discord updates immediately when players join/leave
- Smart data management - Automatic daily and monthly record resets

SERVER PERFORMANCE MONITORING:
- Server FPS monitoring with real-time updates
- Memory usage tracking (used/total system memory)
- Network I/O statistics (bytes in/out per second)
- Server uptime tracking with human-readable format
- Map entities count for performance optimization
- Server status (online/offline indicators with emoji support)

WORLD INFORMATION:
- In-game time display with 12-hour format
- World size and seed information
- Last wipe date with intelligent time elapsed formatting
- Blueprint wipe tracking with separate date tracking
- Next wipe prediction - Automatic first Thursday calculation
- Server protocol version display
- Timezone awareness - Automatic server timezone detection

DISCORD INTEGRATION:
- Smart webhook management with message editing (no spam!)
- Rich embeds with server branding and custom images
- Rate limiting protection with configurable intervals
- Customizable bot name and avatar
- Server header image support with intelligent fallback
- Color-coded status (cyan for online, red for offline)
- Persistent message tracking - Single message that updates continuously

IN-GAME FEATURES:
- !pop command - Display comprehensive server stats to all players
- Configurable cooldown with intelligent time display
- Rich chat formatting with colors and proper spacing
- Automatic broadcasting to all online players
- In-game performance messages - Periodic status updates

===============================================================================
                                 INSTALLATION
===============================================================================

REQUIREMENTS:
- Rust Dedicated Server
- Umod (Oxide) framework
- Discord webhook (optional, for Discord integration)

INSTALLATION STEPS:
1. Copy rPop.cs to your server's oxide/plugins/ directory
2. The plugin will auto-generate its configuration file on first load
3. Configure your Discord webhook (optional) in oxide/config/rPop.json
4. Reload the plugin or restart your server

The plugin will create:
- Configuration: oxide/config/rPop.json
- Data storage: oxide/data/rPop.json

===============================================================================
                              DISCORD SETUP GUIDE
===============================================================================

STEP 1: CREATE DISCORD WEBHOOK
1. Right-click your Discord server
2. Select "Server Settings" ? "Integrations"
3. Click "Webhooks" ? "New Webhook"
4. Name it (e.g., "Rust Server Stats")
5. Select the channel for updates
6. Copy the webhook URL

STEP 2: CONFIGURE PLUGIN
Edit oxide/config/rPop.json:

{
  "Settings": {
    "Discord Webhook URL": "YOUR_WEBHOOK_URL_HERE",
    "Discord Bot Name": "Your Server Name - Live Stats",
    "Performance Message Interval (minutes)": 3.0
  }
}

STEP 3: TEST SETUP
In server console, type: rpop.test

The plugin automatically creates a SINGLE Discord message that updates 
continuously, preventing channel spam.

===============================================================================
                               CONFIGURATION
===============================================================================

The plugin creates a comprehensive configuration file at:
oxide/config/rPop.json

KEY SETTINGS:

CORE SETTINGS:
- Enable !pop Command: true/false - Enable player command
- Command Cooldown (minutes): 5.0 - Cooldown between !pop uses
- Show Last Wipe Date: true/false - Display last server wipe
- Show Next Wipe Date: true/false - Show predicted next wipe
- Show Server Status: true/false - Display online/offline status

POPULATION DISPLAY:
- Show Players Joining: true/false - Display connection queue
- Show Players Sleeping: true/false - Display sleeping players
- Show Admins Online: true/false - Display admin count
- Hide Zero Values: true/false - Hide stats when value is zero
- Show Population Records: true/false - Display peak player records
- Show Total Players Ever: true/false - Show lifetime unique players
- Show Average Connection Time: true/false - Display session times

DISCORD INTEGRATION:
- Discord Webhook URL: "" - Your Discord webhook URL
- Discord Rate Limit (seconds): 1.0 - Time between API calls
- Enable Discord Performance Messages: true/false - Auto Discord updates
- Performance Message Interval (minutes): 3.0 - Update frequency
- Discord Bot Name: "Live Server Statistics" - Bot display name

INSTANT UPDATES:
- Enable Instant Population Updates: true/false - Immediate updates
- Population Update Delay (seconds): 2.0 - Delay to batch events

VISUAL CUSTOMIZATION:
- Use Server Header Image: true/false - Use server's header image
- Fallback Discord Image URL: "" - Backup image when header unavailable
- Use Thumbnail Instead of Image: true/false - Thumbnail vs full image
- Show Discord Image: true/false - Enable/disable Discord images

IN-GAME BROADCASTING:
- Enable In-Game Performance Messages: true/false - Periodic broadcasts
- In-Game Message Interval (minutes): 60.0 - Broadcast frequency

===============================================================================
                                   COMMANDS
===============================================================================

PLAYER COMMANDS:
!pop - Display comprehensive server statistics to all online players
       (Cooldown: configurable, default 5 minutes)

CONSOLE COMMANDS:
rpop.help - Display comprehensive help and setup information
rpop.test - Show current statistics and send test Discord message
rpop.performance - Force immediate Discord performance update
rpop.resetdata - Reset all population records and statistics
rpop.resetmessage - Reset Discord message ID (creates new status message)
rpop.status - Show detailed server status and timer information
rpop.forceconfig - Regenerate configuration file with defaults

===============================================================================
                              WIPE SCHEDULE SYSTEM
===============================================================================

AUTOMATIC WIPE PREDICTION:
- Schedule: First Thursday of each month at 1:00 PM server time
- Timezone Detection: Automatic server timezone identification
- Smart Formatting: Intelligent time remaining display
- Live Updates: Countdown updates in real-time

SUPPORTED TIMEZONES:
- CST/CDT - Central Time
- EST/EDT - Eastern Time  
- MST/MDT - Mountain Time
- PST/PDT - Pacific Time
- UTC/GMT - Universal Time
- Fallback - "Server Time" for unknown zones

===============================================================================
                                TROUBLESHOOTING
===============================================================================

DISCORD INTEGRATION ISSUES:

Messages Not Sending:
1. Check webhook URL in configuration
2. Run "rpop.test" in console to verify setup
3. Check console for error messages
4. Reset message tracking with "rpop.resetmessage"

Rate Limiting:
1. Increase "Discord Rate Limit (seconds)" in config
2. Check for multiple plugins using same webhook
3. Monitor console for rate limit warnings

Message Editing Fails:
1. Run "rpop.resetmessage" in console
2. Run "rpop.performance" to force new message

PERFORMANCE ISSUES:

High CPU Usage:
1. Increase update intervals in configuration
2. Disable instant updates temporarily
3. Check for conflicting plugins

Memory Problems:
1. Check current usage with "rpop.status"
2. Reset data if corrupted with "rpop.resetdata"

FPS Drops:
1. Increase "Performance Message Interval"
2. Disable "Enable In-Game Performance Messages"
3. Monitor with "rpop.status"

CONFIGURATION PROBLEMS:

Missing Options:
1. Reload plugin: oxide.reload rPop
2. Force regeneration: rpop.forceconfig

Invalid JSON:
1. Use online JSON validator to check syntax
2. Verify proper quotation marks and commas
3. Reset to defaults: rpop.forceconfig

DATA TRACKING ISSUES:

Incorrect Player Count:
1. Verify userdata/ folder permissions
2. Check Steam ID validation
3. Use "rpop.test" to verify counting

Population Records Not Updating:
1. Force manual update: rpop.performance
2. Reset all records: rpop.resetdata

Session Time Errors:
1. Restart plugin: oxide.reload rPop
2. Check for player connection issues
3. Verify network stability

===============================================================================
                            PERFORMANCE OPTIMIZATION
===============================================================================

RESOURCE USAGE:
- CPU Impact: < 1% on modern hardware
- Memory Footprint: ~2-5 MB RAM usage
- Network Overhead: Minimal Discord API calls
- Disk I/O: Lightweight JSON file operations

RECOMMENDED SETTINGS:

For HIGH POPULATION SERVERS (100+ players):
{
  "Performance Message Interval (minutes)": 5.0,
  "Population Update Delay (seconds)": 3.0,
  "Discord Rate Limit (seconds)": 2.0
}

For LOW POPULATION SERVERS (< 50 players):
{
  "Performance Message Interval (minutes)": 2.0,
  "Population Update Delay (seconds)": 1.0,
  "Discord Rate Limit (seconds)": 0.5
}

===============================================================================
                              ADVANCED CONFIGURATION
===============================================================================

CUSTOM WIPE SCHEDULE:
If your server doesn't follow standard first Thursday schedule:
{
  "Show Next Wipe Date": false
}

IMAGE CUSTOMIZATION:
{
  "Use Server Header Image": true,
  "Fallback Discord Image URL": "https://your-custom-image.com/image.png",
  "Use Thumbnail Instead of Image": false,
  "Show Discord Image": true
}

MINIMAL DISCORD SETUP (bandwidth-conscious):
{
  "Show Discord Image": false,
  "Performance Message Interval (minutes)": 10.0,
  "Enable Instant Population Updates": false
}

===============================================================================
                                 DATA MANAGEMENT
===============================================================================

AUTOMATIC DATA FILES:
The plugin manages data in oxide/data/rPop.json

SMART RESET SYSTEM:
- Daily Reset: Population records reset at midnight server time
- Monthly Reset: Monthly records reset on the 1st of each month
- Persistent Storage: All-time records and total player counts preserved

PLAYER TRACKING:
- Total Players: Counted from valid Steam ID folders in userdata/
- Session Times: Calculated from active connection duration
- Steam ID Validation: Ensures accurate counting

===============================================================================
                                UPDATE BEHAVIOR
===============================================================================

REGULAR UPDATE CYCLE:
- Discord Updates: Every 3 minutes (configurable)
- In-Game Messages: Every 60 minutes (configurable)
- Population Records: Updated on every player join/leave

INSTANT UPDATE SYSTEM:
- Trigger: Player connections/disconnections
- Smart Batching: 2-second delay to group rapid events
- Timer Reset: Regular update timer resets after instant updates
- Rate Limiting: Prevents Discord API abuse

SERVER STATUS DETECTION:
- Online Detection: Automatic during server startup
- Offline Detection: Triggered during server shutdown
- Final Update: Sends offline status before shutdown
- Visual Indicators: Color-coded Discord embeds and emoji status

===============================================================================
                            SUPPORT & COMMUNITY
===============================================================================

- Report Issues: https://github.com/FtuoilXelrash/rPop/issues
  Bug reports and feature requests

- Discord Support: https://discord.gg/G8mfZH2TMp
  Join our community for help and discussions

- Download Latest: https://github.com/FtuoilXelrash/rPop/releases
  Always get the newest version

===============================================================================
                        DEVELOPMENT & TESTING SERVER
===============================================================================

DARKTIDIA SOLO ONLY - See rPop and other custom plugins in action!
All players are welcome to join our development server where plugins are
tested and refined in a live environment.

SERVER DETAILS:
- Server Name: Darktidia Solo Only | Monthly | 2x | 50% Upkeep | No BP Wipes
- Find Server: https://www.battlemetrics.com/servers/rust/33277489

Experience the plugin live, test configurations, and provide feedback in a
real gameplay setting.

===============================================================================
                                    SUPPORT
===============================================================================

WHEN REPORTING BUGS, INCLUDE:
- Plugin Version: 1.0.0
- Umod Version: [Your Version]
- Server Population: [Typical player count]
- Error Message: [Full console output]
- Configuration: [Relevant config settings]
- Steps to Reproduce: [Detailed steps]

===============================================================================
                                    LICENSE
===============================================================================

This project is licensed under the MIT License.

===============================================================================
                                AUTHOR & CREDITS
===============================================================================

Author: Ftuoil Xelrash
GitHub: https://github.com/FtuoilXelrash/rPop
Discord Support: https://discord.gg/G8mfZH2TMp

Special thanks to:
- Umod Team - For the excellent modding framework
- Rust Community - For feedback and testing
- Contributors - For improvements and bug reports

===============================================================================

Thank you for using rPop! If you find this plugin useful, please consider 
leaving a positive review and sharing it with other server administrators.

For the latest updates and support, visit the GitHub repository or join the
Discord support server.

===============================================================================