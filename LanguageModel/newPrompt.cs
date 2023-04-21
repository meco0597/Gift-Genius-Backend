using Azure;

using Azure.AI.OpenAI;

OpenAIClient client = new OpenAIClient(
	new Uri("https://purviewgpttest.openai.azure.com/"),
	new AzureKeyCredential(Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY")));

// ### If streaming is selected
Response<StreamingChatCompletions> response = await client.GetChatCompletionsStreamingAsync(
	deploymentOrModelName: "gpt-4-32k",
	new ChatCompletionsOptions()
	{
		Messages =
		{
			new ChatMessage(ChatRole.System, @"I am a gifting expert who helps people find creative, useful, and unique gift ideas for a gift recipient. I will be given information about the gift recipients age, interests, relationship to the person seeking the gift ideas, and a max price. I will then provide 10 gift suggestions that match these constraints and are searchable on Amazon."),
			new ChatMessage(ChatRole.User, @"Come up with gift suggestions for my mom. She is in her Fifties and has an interest in cooking, singing. Max price is $100."),
			new ChatMessage(ChatRole.Assistant, @"Gift Idea: Instant Pot Duo 7-in-1 Electric Pressure Cooker
Gift Categories: cooking

Gift Idea: Personalized Recipe Book
Gift Categories: cooking

Gift Idea: Singing Machine Karaoke System
Gift Categories: singing

Gift Idea: Music-themed Kitchen Accessories
Gift Categories: singing, cooking

Gift Idea: Cookware Set
Gift Categories: cooking

Gift Idea: Cookbook Stand
Gift Categories: cooking

Gift Idea: Customized Apron
Gift Categories: cooking

Gift Idea: Online Cooking Class Subscription
Gift Categories: cooking

Gift Idea: Wireless Bluetooth Microphone
Gift Categories: singing

Gift Idea: Herb Garden Starter Kit
Gift Categories: cooking
"),
			new ChatMessage(ChatRole.User, @"Come up with gift suggestions for my cousin. He is in his Twenties and has an interest in soccer, Messi, and NBA. Max price is $100."),
			new ChatMessage(ChatRole.Assistant, @"Gift Idea: Adidas Soccer Ball
Gift Categories: soccer

Gift Idea: Lionel Messi Poster
Gift Categories: soccer, Messi

Gift Idea: Messi Jersey
Gift Categories: soccer, Messi

Gift Idea: NBA 2K22 Video Game
Gift Categories: NBA

Gift Idea: NBA Team Cap
Gift Categories: NBA

Gift Idea: Portable Soccer Goal Set
Gift Categories: soccer

Gift Idea: Messi Biography Book
Gift Categories: soccer, Messi

Gift Idea: Basketball Hoop Laundry Hamper
Gift Categories: NBA

Gift Idea: Soccer Training Equipment
Gift Categories: soccer

Gift Idea: NBA Team T-Shirt
Gift Categories: NBA"),
		},
		Temperature = (float)0.5,
		MaxTokens = 800,
		NucleusSamplingFactor = (float)0.95,
		FrequencyPenalty = 0,
		PresencePenalty = 0,
	});
using StreamingChatCompletions streamingChatCompletions = response.Value;


// ### If streaming is not selected
Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(
	"gpt-4-32k",
	new ChatCompletionsOptions()
	{
		Messages =
		{
			new ChatMessage(ChatRole.System, @"I am a gifting expert who helps people find creative, useful, and unique gift ideas for a gift recipient. I will be given information about the gift recipients age, interests, relationship to the person seeking the gift ideas, and a max price. I will then provide 10 gift suggestions that match these constraints and are searchable on Amazon."),
			new ChatMessage(ChatRole.User, @"Come up with gift suggestions for my mom. She is in her Fifties and has an interest in cooking, singing. Max price is $100."),
			new ChatMessage(ChatRole.Assistant, @"Gift Idea: Instant Pot Duo 7-in-1 Electric Pressure Cooker
Gift Categories: cooking

Gift Idea: Personalized Recipe Book
Gift Categories: cooking

Gift Idea: Singing Machine Karaoke System
Gift Categories: singing

Gift Idea: Music-themed Kitchen Accessories
Gift Categories: singing, cooking

Gift Idea: Cookware Set
Gift Categories: cooking

Gift Idea: Cookbook Stand
Gift Categories: cooking

Gift Idea: Customized Apron
Gift Categories: cooking

Gift Idea: Online Cooking Class Subscription
Gift Categories: cooking

Gift Idea: Wireless Bluetooth Microphone
Gift Categories: singing

Gift Idea: Herb Garden Starter Kit
Gift Categories: cooking
"),
			new ChatMessage(ChatRole.User, @"Come up with gift suggestions for my cousin. He is in his Twenties and has an interest in soccer, Messi, and NBA. Max price is $100."),
			new ChatMessage(ChatRole.Assistant, @"Gift Idea: Adidas Soccer Ball
Gift Categories: soccer

Gift Idea: Lionel Messi Poster
Gift Categories: soccer, Messi

Gift Idea: Messi Jersey
Gift Categories: soccer, Messi

Gift Idea: NBA 2K22 Video Game
Gift Categories: NBA

Gift Idea: NBA Team Cap
Gift Categories: NBA

Gift Idea: Portable Soccer Goal Set
Gift Categories: soccer

Gift Idea: Messi Biography Book
Gift Categories: soccer, Messi

Gift Idea: Basketball Hoop Laundry Hamper
Gift Categories: NBA

Gift Idea: Soccer Training Equipment
Gift Categories: soccer

Gift Idea: NBA Team T-Shirt
Gift Categories: NBA"),
		},
		Temperature = (float)0.5,
		MaxTokens = 800,
		NucleusSamplingFactor = (float)0.95,
		FrequencyPenalty = 0,
		PresencePenalty = 0,
	});

ChatCompletions completions = responseWithoutStream.Value;