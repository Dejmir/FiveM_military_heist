# FiveM_military_heist

Showcase: <a href="https://youtu.be/C4wJqKuE6rU">YT VIDEO</a>

ESX Only

Features:
- Select heist type.
- You can screw up heist in the preparation phase.
- Different missions for each heist.
- NUI for most interactions.
- (silent type) Theres 3 servers inside base, if you lose hacking one server you will fail.
- (loud type) First kill bodyguards then destroy one server.
- Config file for some options.


How to make it works:
- Drop content of release folder to ``\resources\`` and add line: ``start military_heist`` to your server config file.
- In ``\client\client.lua`` at bottom of document inside ``military_heist:policealarm`` trigger, add your own trigger for police.
- Add ``items.sql`` to database.
- Add item: "**fajnyheist_contact**" to another heist or to black shop.
- Add item: "**fajnyheist_documents**" to some store when you can sell it for something.
