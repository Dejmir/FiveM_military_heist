ESX              = nil

Citizen.CreateThread(function()
	while ESX == nil do
		TriggerEvent('esx:getSharedObject', function(obj) ESX = obj end)
		Citizen.Wait(0)
	end
end)

TriggerEvent('esx:getSharedObject', function(obj) ESX = obj end)

RegisterNetEvent('esx_extraitems:fajnyheist_contact')

AddEventHandler('esx_extraitems:fajnyheist_contact', function()
end)

RegisterNetEvent('military_heist:showui:lua')

AddEventHandler('military_heist:showui:lua', function(id, action)
	SendNUIMessage({
		type = action,
		serverid=id
	})
	SetNuiFocus(true, true);
end)

RegisterNUICallback('maincallback', function(data, cb)
	local callback = data.callback;
	local serverid = data.serverid;
	--Used for debug
	--TriggerEvent('chat:addMessage', {
	--	color = { 255, 0, 0},
	--	multiline = false,
	--	args = {callback}
	--})
	if callback == "Accept-1" then TriggerEvent("military_heist:start", "loud", false) closeui() end
	if callback == "Accept-2" then TriggerEvent("military_heist:start", "silent", false) closeui() end
	if callback == "Boom" then TriggerEvent("military_heist:c4callback", "boom") closeui() end
	if callback == "MakeC4" then TriggerEvent("military_heist:c4callback", "good") closeui() end
	if callback == "alarm-good" then TriggerEvent("military_heist:alarmcallback", "good") closeui() end
	if callback == "alarm-bad" then TriggerEvent("military_heist:alarmcallback", "bad") closeui() end
	if callback == "server1-good" then TriggerEvent("military_heist:server1callback", "good") closeui() end
	if callback == "server1-bad" then TriggerEvent("military_heist:server1callback", "bad") closeui() end
	if callback == "server2-good" then TriggerEvent("military_heist:server2callback", "good") closeui() end
	if callback == "server2-bad" then TriggerEvent("military_heist:server2callback", "bad") closeui() end
	if callback == "server3-good" then TriggerEvent("military_heist:server3callback", "good") closeui() end
	if callback == "server3-bad" then TriggerEvent("military_heist:server3callback", "bad") closeui() end
	if callback == "quit" then closeui() end
end)

function closeui()
	SetNuiFocus(false, false)
end

RegisterNetEvent('military_heist:policealarm')

AddEventHandler('military_heist:policealarm', function(id)
	--Police alarm here
end)