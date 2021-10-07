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

exists=$(/opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P $SA_PASSWORD -Q "SET NOCOUNT ON SELECT case when name is not null then 1 else 0 end as existe from master.sys.databases where name = 'programa_gestao'")
if [[ $exists -eq 1 ]];
then
    echo "The database isn't empty. No one script will be executed"
else
    # Create default database
    /opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P $SA_PASSWORD -Q "CREATE DATABASE programa_gestao"

    # Run migrations
    for foo in /scripts/*.sql
        do /opt/mssql-tools/bin/sqlcmd -d programa_gestao -U sa -P $SA_PASSWORD -l 30 -e -i $foo
    done
fi

# So that the container doesn't shut down, sleep this thread
wait $pid
exit 0
