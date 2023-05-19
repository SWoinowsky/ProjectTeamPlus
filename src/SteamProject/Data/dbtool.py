import pyodbc       # pip install pyodbc
import click        # pip install click

@click.command()
@click.argument('server')
@click.argument('db-name')
@click.argument('username')
@click.argument('password')
@click.argument('path-to-script')
@click.option('--dry-run','-n',default=False,is_flag=True)
def cli(server,db_name,username,password,path_to_script,dry_run):
    """
    Use ODBC to run a .sql script on a database in a server, i.e. run it on Azure
    SERVER needs to look like: myserver.database.windows.net
    DB-NAME is like testdeployAuth
    USERNAME is the db server admin username
    PASSWORD is the db server admin password
    PATH-TO-SCRIPT is the .sql script to run.  Make sure it has semicolons (;) ending each statement
    and there are no GO or compound statements that have more than one semicolon. This
    is because we are only doing a simple split(';') to split commands so we can execute
    them one by one.  We can't send the entire script as one statement so we have to break
    them apart.  Do a dry run to see how it split them.
    The script is also assumed to be in UTF-8 encoding, possibly with a BOM
    """
    run_sql_script(server,db_name,username,password,path_to_script,dry_run)

def run_sql_script(server,db_name,username,password,path_to_script,dry_run):
    server = 'tcp:' + server
    if not dry_run:
        cnxn = pyodbc.connect('DRIVER={{ODBC Driver 17 for SQL Server}};SERVER={};DATABASE={};UID={};PWD={}'.format(server,db_name,username,password), autocommit=True)
        cursor = cnxn.cursor()
    with open(path_to_script,'r',encoding='utf-8-sig') as fin:
        # split on semicolons to facilitate running each command separately, which is the model
        # we need to follow
        script = fin.read()
        for sqlcmd in script.split(';'):
            sqlcmd = sqlcmd.strip()
            if sqlcmd:
                if dry_run:
                    print(sqlcmd)
                else:
                    cursor.execute(sqlcmd)
    if not dry_run:
        cnxn.close()

if __name__ == '__main__':
    cli()
