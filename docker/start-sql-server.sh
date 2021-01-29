# Baseado em: https://github.com/Microsoft/mssql-docker/issues/11
/opt/mssql/bin/sqlservr &
pid=$!

echo "Waiting for MS SQL to be available ⏳"
is_up=1
while [ $is_up -ne 0 ] ; do 
    echo "Trying to connect in " $(date) 
    /opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P $SA_PASSWORD -Q "SET NOCOUNT ON SELECT \"YAY WE ARE UP\" , @@servername"
    is_up=$?
    sleep 5 
done

for foo in /scripts/*.sql
    do /opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -l 30 -e -i $foo
done
# So that the container doesn't shut down, sleep this thread
wait $pid
exit 0
