import Link from "next/link";
import Image from "next/image";
import client from "../../helpers/axios-client";
import styles from "../../styles/Home.module.css";

export default function GiftSuggestions({ suggestions }) {
    return (
        <div style={{ display: "flex", flexDirection: "column" }}>

            <h1 style={{ marginTop: '50px', textAlign: "center" }}>
                Your Gift Ideas!
            </h1>

            <div className={styles.suggestions}>
                {suggestions.map((suggestion) => (
                    <Link
                        href={suggestion.link}
                        title={suggestion.title}
                        key={suggestion.title}
                        className={styles.card}
                        target="_blank"
                    >
                        <Image
                            className="my-12 w-full"
                            src={suggestion.thumbnailUrl}
                            alt={suggestion.title}
                            style={{ height: 'auto', maxHeight: 200, objectFit: 'contain', position: 'relative' }}
                            width={150}
                            height={150}
                            priority
                        />

                        {truncate(suggestion.title)}
                    </Link>
                ))}
            </div>
        </div>
    );
};

const truncate = (input) =>
    input?.length > 100 ? `${input.substring(0, 100)}...` : input;

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

    let response = await client.post(process.env.BACKEND_URL + '/api/giftsuggestions', {
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