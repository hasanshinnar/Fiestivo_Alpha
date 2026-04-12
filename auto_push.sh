#!/bin/sh
SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &> /dev/null && pwd)
cd "$SCRIPT_DIR"

git add .
echo 'Enter the commit message:'
read commitMessage

git commit -m "$commitMessage"

echo 'Enter the name of the branch:'
read branch

git push origin $branch
echo 'Push successful! Press Enter to exit.'
read