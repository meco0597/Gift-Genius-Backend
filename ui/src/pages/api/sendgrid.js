import axios from 'axios';

export default async function handler(req, res) {
    if (req.method === 'POST') {
        try {
            const { email, suggestions } = req.body;
            const response = await axios.post(
                'https://api.sendgrid.com/v3/mail/send',
                {
                    from: {
                        email: 'info@givr.ai',
                    },
                    personalizations: [
                        {
                            to: [
                                {
                                    email: email,
                                },
                            ],
                            dynamic_template_data: {
                                suggestions: suggestions.map((suggestion) => ({
                                    title: suggestion.title,
                                    linkUrl: suggestion.link,
                                    imgUrl: suggestion.thumbnailUrl,
                                })),
                            },
                        },
                    ],
                    template_id: 'd-cbf13184c1dd4c52b66250e2dcdd92bd',
                },
                {
                    headers: {
                        Authorization: `Bearer ${process.env.SENDGRID_API_KEY}`,
                        'Content-Type': 'application/json',
                    },
                }
            );

            console.log('Email submitted:', email);
            console.log('API response:', response.data);

            res.status(200).json({ success: true });
        } catch (error) {
            console.error('Error submitting email:', error);
            res.status(500).json({ success: false, error: 'Failed to submit email' });
        }
    } else {
        res.status(404).json({ error: 'Invalid request method' });
    }
}
