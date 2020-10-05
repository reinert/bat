mkdir .data
mkdir .data/pgsql
mkdir .data/sqlite
cp .env-template .env
sed -i "s:\$USR:$USER:g" .env
USRID=`id -u`
GRPID=`id -g`
sed -i "s:\$UID:$USRID:g" .env
sed -i "s:\$GID:$GRPID:g" .env
