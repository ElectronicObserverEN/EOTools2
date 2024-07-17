#!/usr/bin/bash 
echo "Pushing data repo"
cd ./Data/DataRepo/ 
git add -A 
git pull 
git commit -m "$1"
git push