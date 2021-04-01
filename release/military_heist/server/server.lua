ESX = nil

TriggerEvent('esx:getSharedObject', function(obj) ESX = obj end)

ESX.RegisterUsableItem('fajnyheist_contact', function(playerId)
	local xPlayer = ESX.GetPlayerFromId(playerId)
	TriggerClientEvent('esx_extraitems:fajnyheist_contact', playerId)
	TriggerClientEvent("military_heist:contact:start", playerId)
end)