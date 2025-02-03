# /Campaigns

**CREATE**

```json
{
  "Name": "Campaign Name",
  "PlayerIds": [
    4,
    6,
    7
  ]
}
```

**GET**

```json
{
  "Items": [
    {
      "Id": 5,
      "Name": "Campaign Name",
      "Gamemaster": "Peter",
      "Players": [
        "Matt",
        "Steve"
      ]
    },
    {
      "Id": 6,
      "Name": "Campaign Name 2",
      "Gamemaster": "Togel",
      "Players": [
        "Shiro",
        "Tim",
        "Rina"
      ]
    }
  ]
}
```

# /Campaigns/{CampaignId}

**GET**

```json
{
  "Name": "Campaign Name",
  "PlayerIds": [
    4,
    6,
    7
  ]
}
```

**PATCH**

```json
{
  "Name": "Different Campaign Name",
  "PlayerIds": [
    1,
    2
  ]
}
```

# /Campaigns/{CampaignId}/ActiveMap

**GET**

```json
{
  "MapId": 5
}
```

**PATCH**

```json
{
  "MapId": 5
}
```

# /Campaigns/{CampaignId}/Characters

**GET**

```json
{
  "Items": [
    {
      "CharacterId": 2,
      "CharacterName": "Barodil",
      "PlayerName": "Togel",
      "Image": [
        0,
        4,
        3
      ]
    },
    {
      "CharacterId": 3,
      "CharacterName": "Berserk",
      "PlayerName": "Ryo",
      "Image": [
        1,
        2,
        4
      ]
    }
  ]
}
```

# /Campaigns/{CampaignId}/Chat

**POST**

```json
{
  "SenderId": 1,
  "ReceiverId": 2,
  "Message": "Text"
}
```

// Hint: ReceiverId is optional

# /Campaigns/{CampaignId}/Chat/Users

**GET**

```json
{
  "Items": [
    {
      "UserId": 2,
      "Name": "Gamemaster"
    },
    {
      "UserId": 4,
      "Name": "Barodil"
    }
  ]
}
```

# /Campaigns/{CampaignId}/Maps

**GET**

```json
{
  "Items": [
    {
      "Id": 1,
      "Name": "First Encounter",
      "Image": [
        0,
        1,
        2
      ]
    },
    {
      "Id": 2,
      "Name": "City",
      "Image": [
        3,
        2,
        1
      ]
    }
  ]
}
```

**POST**

```json
{
  "Name": "Map Name",
  "Image": [
    0,
    1
  ],
  "Grid": {
    "IsActive": true,
    "Size": 25
  }
}
```

# /Campaigns/{CampaignId}/Maps/{MapId}

**DELETE**

**GET**

```json
{
  "Name": "Map Name",
  "Image": [
    2,
    3
  ],
  "Grid": {
    "IsActive": true,
    "Size": 25
  }
}
```

# /Campaigns/{CampaignId}/Maps/{MapId}/Grid

**PATCH**

```json
{
  "IsActive": true,
  "Size": 25
}
```

# /Campaigns/{CampaignId}/Maps/{MapId}/Name

**PATCH**

```json
{
  "Name": "New Map Name"
}
```

# /Campaigns/{CampaignId}/Maps/{MapId}/Script

**GET**

```json
{
  "Text": "#Heading"
}
```

**PUT**

```json
{
  "Text": "#Heading"
}
```

# /Campaigns/{CampaignId}/Maps/{MapId}/Tokens

**GET**

```json
{
  "Items": [
    {
      "TokenId": 2,
      "UserId": 5,
      "X": 2,
      "Y": 2,
      "Name": "My Name",
      "Image": [
        0,
        1,
        2
      ]
    },
    {
      "TokenId": 3,
      "UserId": 2,
      "X": 5,
      "Y": 4,
      "Name": "Name #2",
      "Image": [
        2,
        3,
        4
      ]
    }
  ]
}
```

# /Campaigns/{CampaignId}/Maps/{MapId}/Tokens/{TokenId}

**DELETE**

**PATCH**

```json
{
  "X": 31,
  "Y": 45
}
```

# /Campaigns/{CampaignId}/Maps/{MapId}/Tokens/Characters/{CharacterId}

**POST**

```json
{
  "X": 1,
  "Y": 3
}
```

# /Campaigns/{CampaignId}/Maps/{MapId}/Tokens/Monsters/{MonsterId}

**POST**

```json
{
  "X": 13,
  "Y": 23
}
```

# /Campaigns/{CampaignId}/MouseIndicators

**POST**

```json
{
  "X": 2,
  "Y": 4
}
```

# /Campaigns/{CampaignId}/Rolls

**POST**

```json
{
  "UserId": 13,
  "Dice": 4
}
```

# /Campaigns/{CampaignId}/Scripts

**GET**

```json
{
  "Items": [
    {
      "MapId": 1,
      "MapName": "Map 1"
    },
    {
      "MapId": 3,
      "MapName": "Map 2"
    }
  ]
}
```

# /Campaigns/{CampaignId}/Sounds/{SoundId}

**DELETE**

**POST**

```json
{
  "IsFaded": false,
  "IsLooped": true
}
```

# /Campaigns/{CampaignId}/Users/{UserId}/Characters

**GET**

```json
{
  "Items": [
    {
      "Name": "Aragorn",
      "Image": [
        0,
        2
      ]
    },
    {
      "Name": "Holger",
      "Image": [
        2,
        1
      ]
    }
  ]
}
```

**POST**

```json
{
  "Name": "Aragorn",
  "Image": [
    0,
    2
  ]
}
```

# /DungeonsAndDragons5e/Monsters

**GET**

```json
{
  "Items": [
    {
      "Id": 2,
      "Name": "Beobachter",
      "Icon": [
        0,
        1,
        2
      ],
      "SizeCategory": 2,
      "Type": "Tier",
      "Alignment": "Chaotisch böse",
      "ChallengeRating": 0.25
    },
    {
      "Id": 4,
      "Name": "Grick",
      "Icon": [
        2,
        3,
        4
      ],
      "SizeCategory": 1,
      "Type": "Monstrosität",
      "Alignment": "Neutral",
      "ChallengeRating": 1.0
    }
  ]
}
```

# /DungeonsAndDragons5e/Monsters/{MonsterId}

**GET**

```json
{
  "Name": "Beobachter",
  "Image": [
    0,
    1,
    2
  ],
  "SizeCategory": 2,
  "Type": "Monstrosität",
  "Alignment": "Chaotisch böse",
  "ArmorClass": 12,
  "HitPoints": 50,
  "HitDice": "2xD10",
  "Speed": "15m",
  "Strength": 2,
  "Dexterity": 3,
  "Constitution": 1,
  "Intelligence": -2,
  "Wisdom": -1,
  "Charisma": 0,
  "SavingThrowStrength": 2,
  "SavingThrowDexterity": 2,
  "SavingThrowConstitution": 1,
  "SavingThrowIntelligence": -2,
  "SavingThrowWisdom": -1,
  "SavingThrowCharisma": 0,
  "Acrobatics": 2,
  "AnimalHandling": 2,
  "Arcana": 3,
  "Athletics": 1,
  "Deception": 2,
  "History": 0,
  "Insight": 0,
  "Intimidation": 1,
  "Investigation": 2,
  "Medicine": 3,
  "Nature": 2,
  "Perception": 1,
  "Performance": 2,
  "Persuasion": 3,
  "Religion": 0,
  "SlightOfHand": -1,
  "Stealth": -2,
  "Survival": 1,
  "DamageResistances": "Blitz",
  "DamageImmunities": "Gift, Feuer",
  "ConditionImmunities": "Bezaubert, vergiftet",
  "Senses": "* **Passive Wahrnehmung** 13",
  "Languages": "Gemeinsprache, Goblinisch",
  "ChallengeRating": 0.25,
  "Experience": 50,
  "Actions": "* **Krummsäbel.** *Nahkampf-Waffenangriff:* +3 zum Treffen, Reichweite 1,50m, ein Ziel. *Treffer:* 4 (1W6 + 1) Hiebschaden."
}
```
