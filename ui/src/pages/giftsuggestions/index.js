import { useState } from "react";
import Link from "next/link";
import Image from "next/image";
import client from "../../helpers/axios-client";
import { useEffect } from "react";

import styles from "../../styles/Home.module.css";
import { useRequest } from "../../helpers/use-request";
import MsgErrors from "../../helpers/nonValidationErr";
import axiosSrv from "../../helpers/axios-client";
import { headers } from "../../../next.config";

export default function GiftSuggestions({ suggestions }) {
    return (
        <div className={styles.container}>
            {suggestions.map((suggestion) => (
                <div key={suggestion.title} className={styles.card}>
                    <Image
                        className="my-12 w-full"
                        src={suggestion.thumbnailUrl}
                        alt={suggestion.title}
                        width={100}
                        height={100}
                        priority
                    />
                    <Link
                        href={suggestion.link}
                        title={suggestion.title}
                        target="_blank"
                    >
                        {suggestion.title}
                    </Link>
                </div>
            ))}
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

    let response = await client.post('http://givr.centralus.cloudapp.azure.com:5001/api/giftsuggestions', {
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