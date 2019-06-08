export interface Quote {
    uuid: string;
    author: string;
    text: string;
}

export function emptyQuote(): Quote {
    return { uuid: '', author: '', text: '' };
}
