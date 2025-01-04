﻿using DataTransfer.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Hubs;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController(SQLDatabase dbContext, IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(int campaignId)
    {
        var campaign = await dbContext.Campaigns
            .Include(c => c.Gamemaster)
            .Include(c => c.Players)
            .ThenInclude(u => u.Characters)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign == null) return NotFound(campaignId);

        List<ChatUser> characters =
        [
            new(0, "Alle"),
            new(campaign.Gamemaster.Id, "Spielmeister")
        ];

        foreach (var player in campaign.Players)
        {
            if (player.Characters.LastOrDefault() is { } character)
            {
                var chatUser = new ChatUser(player.Id, character.Name);
                characters.Add(chatUser);
            }
        }

        var chatUsers = new ChatUsersDto(characters);

        return Ok(chatUsers);
    }

    [HttpPost]
    public async Task<IActionResult> Send(ChatMessageDto payload)
    {
        var campaign = await dbContext.Campaigns
            .Include(c => c.Gamemaster)
            .Include(c => c.Characters)
            .FirstOrDefaultAsync(c => c.Id == payload.CampaignId);

        if (campaign is null) return NotFound(payload.CampaignId);

        var character = await dbContext.Characters
            .Include(c => c.User)
            .Include(c => c.Campaign)
            .OrderBy(c => c.Id)
            .LastOrDefaultAsync(c => c.Campaign.Id == campaign.Id && c.User.Id == payload.SenderId);

        if (character is null) return NotFound(payload.SenderId);

        var isPrivate = payload.ReceiverId is not null;
        var isSenderGameMaster = payload.SenderId == campaign.Gamemaster.Id;
        var senderName = isSenderGameMaster ? "Spielmeister" : character.Name;
        var senderImage = isSenderGameMaster ? GameMasterImage : Convert.ToBase64String(character.Image);

        var chatMessage = new ChatMessageEventArgs(
            DateTime.Now,
            ChatMessageType.Message,
            payload.SenderId,
            senderName,
            payload.Text,
            senderImage,
            isPrivate);

        if (isPrivate)
        {
            // ToDo: Only send to respective client
        }
        else
        {
            await campaignUpdateHub.Clients.All.ChatMessageReceived(chatMessage);
        }

        return Ok(chatMessage);
    }

    private const string GameMasterImage =
        "data:image/jpeg;base64,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/GMzLdsAADowaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjYtYzA2NyA3OS4xNTc3NDcsIDIwMTUvMDMvMzAtMjM6NDA6NDIgICAgICAgICI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgICAgICAgICAgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iCiAgICAgICAgICAgIHhtbG5zOnN0RXZ0PSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VFdmVudCMiCiAgICAgICAgICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgICAgICAgICAgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIgogICAgICAgICAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZXhpZj0iaHR0cDovL25zLmFkb2JlLmNvbS9leGlmLzEuMC8iPgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1IChXaW5kb3dzKTwveG1wOkNyZWF0b3JUb29sPgogICAgICAgICA8eG1wOkNyZWF0ZURhdGU+MjAyNS0wMS0wNFQxMjoyMDoyMyswMTowMDwveG1wOkNyZWF0ZURhdGU+CiAgICAgICAgIDx4bXA6TWV0YWRhdGFEYXRlPjIwMjUtMDEtMDRUMTI6MjA6MjMrMDE6MDA8L3htcDpNZXRhZGF0YURhdGU+CiAgICAgICAgIDx4bXA6TW9kaWZ5RGF0ZT4yMDI1LTAxLTA0VDEyOjIwOjIzKzAxOjAwPC94bXA6TW9kaWZ5RGF0ZT4KICAgICAgICAgPHhtcE1NOkluc3RhbmNlSUQ+eG1wLmlpZDo1MDNlMTQ4Yi05NTgzLTE2NDQtYmZhNi0yYzY2ZDkyYzIzNzY8L3htcE1NOkluc3RhbmNlSUQ+CiAgICAgICAgIDx4bXBNTTpEb2N1bWVudElEPmFkb2JlOmRvY2lkOnBob3Rvc2hvcDplM2M2NGNmZC1jYThkLTExZWYtYmM4Yy1jOTQyNDRiY2ViYTM8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDozOWY4OGE4MC1hOTkxLTliNDctYjViZC1hZGExMTk1Y2U5YjU8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6MzlmODhhODAtYTk5MS05YjQ3LWI1YmQtYWRhMTE5NWNlOWI1PC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDI1LTAxLTA0VDEyOjIwOjIzKzAxOjAwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cyk8L3N0RXZ0OnNvZnR3YXJlQWdlbnQ+CiAgICAgICAgICAgICAgIDwvcmRmOmxpPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5zYXZlZDwvc3RFdnQ6YWN0aW9uPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6aW5zdGFuY2VJRD54bXAuaWlkOjUwM2UxNDhiLTk1ODMtMTY0NC1iZmE2LTJjNjZkOTJjMjM3Njwvc3RFdnQ6aW5zdGFuY2VJRD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OndoZW4+MjAyNS0wMS0wNFQxMjoyMDoyMyswMTowMDwvc3RFdnQ6d2hlbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OnNvZnR3YXJlQWdlbnQ+QWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICAgICA8c3RFdnQ6Y2hhbmdlZD4vPC9zdEV2dDpjaGFuZ2VkPgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDxkYzpmb3JtYXQ+aW1hZ2UvcG5nPC9kYzpmb3JtYXQ+CiAgICAgICAgIDxwaG90b3Nob3A6Q29sb3JNb2RlPjM8L3Bob3Rvc2hvcDpDb2xvck1vZGU+CiAgICAgICAgIDxwaG90b3Nob3A6SUNDUHJvZmlsZT5zUkdCIElFQzYxOTY2LTIuMTwvcGhvdG9zaG9wOklDQ1Byb2ZpbGU+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjE8L2V4aWY6Q29sb3JTcGFjZT4KICAgICAgICAgPGV4aWY6UGl4ZWxYRGltZW5zaW9uPjY0PC9leGlmOlBpeGVsWERpbWVuc2lvbj4KICAgICAgICAgPGV4aWY6UGl4ZWxZRGltZW5zaW9uPjY0PC9leGlmOlBpeGVsWURpbWVuc2lvbj4KICAgICAgPC9yZGY6RGVzY3JpcHRpb24+CiAgIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz7pyZptAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAACWPSURBVHjaNLlXrOVZdp+3dvynk8+55+ZQt+pW7pyHw54ZcWRBA0gkLdIUJRlWoASKoGGDhi0CkmUDhgFBgiCJki3alh4sWiSl0cg0OU1O4oTO1T3dXd3VXVVd4aa6+eRz/mnH5Yei1tN+3Rtr7fXh95H//B9+VyNN4lgEgZQyTCIZylozopTO0uJ0PK7PtRdXa/3hgdNMG5vPiizNlfYAggMygIBLwQV6o4pCa2OMIYQQAIJACEFESikiFmXhnEdE75z3BAAA0HsHQBApAAYyDKOIEkoI4VwIIYWUQKi3zjlHCHHGaKUQABGtsYgYScY/vb/LhZhf6FYrNcqUm05mRRFXqkBwmGVBs2U7ZTNJ1hbPC6SBBO9gNtFno3z76Cw9G+GomLqcM8E5DbhgARNR6DwarQkQQgggWu+s8cpCqTQgAiIiRUDvvbUWERDBO8+5rSKt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";
}