
"use client" // Error components must be Client components

export default function Error({ error, reset }) {
    return (
        <div>
            <h2>Something went wrong!</h2>
            <p>{error}</p>
            <button onClick={() => reset()}>Try again</button>
        </div>
    )
}