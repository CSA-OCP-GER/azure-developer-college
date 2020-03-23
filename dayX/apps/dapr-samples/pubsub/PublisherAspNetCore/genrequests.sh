#!/bin/sh

for ((i=0;i<$1;i++))
do
    curl -X POST "http://localhost:5000/Publisher" -H "accept: */*" -H "Content-Type: application/json" -d "\"Hello World $i, dapr\""
done