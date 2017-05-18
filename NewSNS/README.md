https://gitlab.com/TRM79/Some-wonderful-project<br/>
<br/>
  SSH: git@gitlab.com:TRM79/Some-wonderful-project.git<br/>
HTTPS: https://gitlab.com/TRM79/Some-wonderful-project.git<br/>
<br/>
The repository for this project is empty<br/>
If you already have files you can push them using command line instructions below.<br/>
Otherwise you can start with adding a README, a LICENSE, or a .gitignore to this project.<br/>
You will need to be owner or have the master permission level for the initial push, as the master branch is automatically protected.<br/>
  https://gitlab.com/TRM79/Some-wonderful-project/new/master?file_name=README.md<br/>
  https://gitlab.com/TRM79/Some-wonderful-project/new/master?commit_message=Add+license&file_name=LICENSE<br/>
  https://gitlab.com/TRM79/Some-wonderful-project/new/master?commit_message=Add+.gitignore&file_name=.gitignore<br/>
  <br/>
Command line instructions<br/>
<br/>
<br/>
Git global setup<br/>
<br/>
git config --global user.name "Roman Tarasiuk"<br/>
git config --global user.email "roman.tarasiuk.l@gmail.com"<br/>
<br/>
Create a new repository<br/>
<br/>
git clone https://gitlab.com/TRM79/Some-wonderful-project.git<br/>
cd Some-wonderful-project<br/>
touch README.md<br/>
git add README.md<br/>
git commit -m "add README"<br/>
git push -u origin master<br/>
<br/>
Existing folder or Git repository<br/>
<br/>
cd existing_folder<br/>
git init<br/>
git remote add origin https://gitlab.com/TRM79/Some-wonderful-project.git<br/>
git add .<br/>
git commit<br/>
git push -u origin master<br/>
<br/>
<br/>
<br/>
====<br/>
<br/>
https://github.com/Roman-Tarasiuk/M3U8_Player<br/>
<br/>
Quick setup — if you’ve done this kind of thing before<br/>
github-windows://openRepo/https://github.com/Roman-Tarasiuk/M3U8_Player<br/>
or HTTPS: https://github.com/Roman-Tarasiuk/M3U8_Player.git<br/>
     SSH: git@github.com:Roman-Tarasiuk/M3U8_Player.git<br/>
<br/>
We recommend every repository include a README, LICENSE, and .gitignore.<br/>
  https://github.com/Roman-Tarasiuk/M3U8_Player/new/master?readme=1<br/>
  https://github.com/Roman-Tarasiuk/M3U8_Player/new/master?filename=LICENSE.md<br/>
  https://github.com/Roman-Tarasiuk/M3U8_Player/new/master?filename=.gitignore<br/>
<br/>
<br/>
…or create a new repository on the command line<br/>
<br/>
echo "# M3U8_Player" >> README.md<br/>
git init<br/>
git add README.md<br/>
git commit -m "first commit"<br/>
git remote add origin https://github.com/Roman-Tarasiuk/M3U8_Player.git<br/>
git push -u origin master<br/>
<br/>
<br/>
…or push an existing repository from the command line<br/>
<br/>
git remote add origin https://github.com/Roman-Tarasiuk/M3U8_Player.git<br/>
git push -u origin master<br/>
<br/>
<br/>
…or import code from another repository<br/>
You can initialize this repository with code from a Subversion, Mercurial, or TFS project.