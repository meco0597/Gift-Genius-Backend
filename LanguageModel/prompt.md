I am going to give you a prompt describing a recipient of a gift. Your job is to generate gift suggestions in json format providing the gift name, a description for the reasoning of the gift, interest categories of the gift, and the estimated price of the gift. An example prompt is:

"5 birthday gift ideas for a 52-year-old Female with an interest in cooking and singing, between $25 and $100"

Then you would respond in json markdown:

```json
[
    {
        "giftIdea": "Cookbook by a celebrity chef",
        "giftCategories": [
            "cooking"
        ],
        "estimatedPrice": "30-45",
        "giftDescription": "A collection of recipes, instructions, and information about the preparation and serving of foods. This gift will allow them to explore unique cooking recipes."
    },
    {
        "giftIdea": "Custom Cutting Board",
        "giftCategories": [
            "cooking"
        ],
        "estimatedPrice": "20-80",
        "giftDescription": "A kitchen utensil used as a protective surface on which to cut or slice things. Add a personal touch by customizing it with their name or initials."
    },
    {
        "giftIdea": "Songwriting Notebook",
        "giftCategories": [
            "singing"
        ],
        "estimatedPrice": "10-20",
        "giftDescription": "A collection of words, notes, and thoughts from successful songwriters who've had some success themselves, it offers advice on how to create your own masterpiece, and a collection of great ideas to help you get there."
    },
    {
        "giftIdea": "Poster of a Musical Artist",
        "giftCategories": [
            "singing"
        ],
        "estimatedPrice": "10-25",
        "giftDescription": "A decorative piece that can liven up a space and display personality and musical taste."
    },
    {
        "giftIdea": "Wine Decanter",
        "giftCategories": [
            "cooking"
        ],
        "estimatedPrice": "30-50",
        "giftDescription": "A glass vessel that is used to help aerate wine. For the home cook who serves wine with every meal."
    }
]
```

My first prompt is:

"5 Valentines gift ideas for a 28-year-old female with an interest in sewing, fitness, and piano, between $75 and $150"
