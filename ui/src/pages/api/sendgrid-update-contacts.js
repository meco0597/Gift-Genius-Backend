import axios from 'axios';

export default async function handler(req, res) {
    if (req.method === 'PUT') {
        try {
            const { contacts } = req.body;
            const response = await axios.put(
                'https://api.sendgrid.com/v3/marketing/contacts',
                { contacts },
                {
                    headers: {
                        Authorization: `Bearer ${process.env.SENDGRID_API_KEY}`,
                        'Content-Type': 'application/json',
                    },
                }
            );

            console.log('Update Contacts API response:', response.data);

            res.status(200).json({ success: true });
        } catch (error) {
            console.error('Error updating contacts:', error);
        }
    } else {
        res.status(404).json({ error: 'Invalid request method' });
    }
}
