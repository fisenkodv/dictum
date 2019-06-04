import { Quote } from '../models';

export class QuotesApi {
    public static getRandomQuote(): Promise<Quote> {
        return fetch('/dictum', {
            method: 'get'
        })
            .catch(error => console.error(error))
            .then(response =>
                response instanceof Response ? response.json().then(r => Promise.resolve(r)) : Promise.reject(response)
            );
    }
}
