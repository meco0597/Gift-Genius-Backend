import Link from "next/link";
import Image from "next/image";
import client from "../../helpers/axios-client";
import styles from "../../styles/Home.module.css";
import AmazonProductContent from "../../components/AmazonProductContent";

export default function GiftSuggestions({ suggestions }) {

    // randomize products array
    suggestions.sort(() => Math.random() - 0.5)

    return (
        <div style={{ display: "flex", flexDirection: "column" }}>

            <h1 style={{ marginTop: '20px', textAlign: "center" }}>
                Your Gift Ideas!
            </h1>

            <div className={styles.suggestions}>
                {suggestions.map((suggestion) => (
                    <div
                        title={suggestion.title}
                        key={suggestion.title}
                        className={styles.card}
                    >
                        <div style={{ width: '50%', height: '0', paddingBottom: '40%', position: 'relative' }}>
                            <Image
                                className="my-12 w-full"
                                src={suggestion.thumbnailUrl}
                                alt={suggestion.title}
                                style={{ objectFit: "contain", position: 'absolute', top: '0', left: '0', width: '100%', height: '100%' }}
                                width={150}
                                height={150}
                                href={suggestion.link}
                                target="_blank"
                                priority
                            />
                        </div>

                        <AmazonProductContent suggestion={suggestion} />
                    </div>
                ))}
            </div>
        </div>
    );
};

export async function getServerSideProps(context) {
    if (!context.query.associatedRelationship ||
        !context.query.pronoun ||
        !context.query.associatedAge ||
        !context.query.associatedInterests ||
        !context.query.maxPrice) {
        const { res } = context
        res.writeHead(301, { Location: '/' })
        res.end()
        return true
    }

    let response = await client.post(process.env.BACKEND_URL + '/api/giftsuggestions/amazon', {
        associatedRelationship: context.query.associatedRelationship,
        pronoun: context.query.pronoun,
        associatedAge: context.query.associatedAge,
        associatedInterests: context.query.associatedInterests.split(','),
        maxPrice: context.query.maxPrice,
    });

    let suggestions = response.data;

    return {
        props: {
            suggestions
        },
    };
}