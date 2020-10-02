mkdir .data
mkdir .data/pgsql
mkdir .data/sqlite
cp .env-template .env
sed -i "s:\$USR:$USER:g" .env
UID=`id -u`
GID=`id -g`
sed -i "s:\$UID:$UID:g" .env
sed -i "s:\$GID:$GID:g" .env
