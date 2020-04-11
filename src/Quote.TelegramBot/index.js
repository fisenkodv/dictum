const fetch = require('node-fetch');
const { random, includes } = require('lodash');
const quoteApiUrl = 'https://api.fisenko.net/quotes';

const enKeywords = ['though', 'quote', 'dictum', 'wise', 'more'];
const ruKeywords = ['цитата', 'мысль', 'еще', 'ещё'];

function getLanguage(query) {
  if (enKeywords.some(keyword => includes(query, keyword))) return 'en';
  else if (ruKeywords.some(keyword => includes(query, keyword))) return 'ru';
  return null;
}

async function getQuote(language) {
  try {
    const url = `${quoteApiUrl}?l=${language}`;
    const data = await fetch(url);
    const json = await data.json();
    const quote = json.text;
    const author = json.author;
    return `<b>${quote}</b>\n\u2014 <i>${author}</i>`;
  } catch (err) {
    console.error('Fail to fetch data: ' + err);
    return language === 'en'
      ? 'Oops, something went wrong. Try one more time?'
      : 'Ой, что-то пошло не так. Может ещё раз?';
  }
}

function getHelp() {
  const help =
    '<b>To get a quote send any of the following:</b> <i>quote, though, dictum, more</i>\n' +
    '<b>Чтобы получить цитату отправьте любую из следующих фраз:</b> <i>цитата, мысль, еще</i>';

  return help;
}

exports.handler = async event => {
  const body = JSON.parse(event.body);
  const text = body.message.text.trim().toLowerCase();

  let botMessage;
  if (text === '/help') {
    botMessage = getHelp();
  }

  const language = getLanguage(text);
  if (language !== null) botMessage = await getQuote(language);
  else botMessage = getHelp();

  const msg = {
    method: 'sendMessage',
    parse_mode: 'HTML',
    chat_id: body.message.chat.id,
    text: botMessage,
    reply_markup: JSON.stringify({
      keyboard: [[{ text: 'More quote' }], [{ text: 'Ещё цитату' }]]
    })
  };

  const response = {
    statusCode: 200,
    headers: {
      'Content-Type': 'application/json; charset=utf-8'
    },
    body: JSON.stringify(msg),
    isBase64Encoded: false
  };
  return response;
};
