#!/usr/bin/env bash

cd $(pwd)/$(dirname "$0")/../src/Quote.TelegramBot

printf "Building AWS Lambda...\n\n"
rm function.zip
npm i
zip -r function.zip
aws lambda update-function-code --function-name quote-telegram-bot --zip-file fileb://function.zip
