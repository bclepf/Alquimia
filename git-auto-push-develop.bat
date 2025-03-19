@echo off
echo ==============================
echo  Git - Push Automático para Alquimia (branch develop)
echo ==============================

:: Configurar nome e email (opcional, se não configurado antes)
git config --global user.name "Seu Nome"
git config --global user.email "seuemail@exemplo.com"

:: Adicionar todos os arquivos
git add .

:: Commit com mensagem padrão
git commit -m "Atualizando projeto"

:: Puxar do repositório remoto (para evitar conflitos)
git pull origin develop --allow-unrelated-histories

:: Push para GitHub
git push -u origin develop

echo ==============================
echo  Push realizado com sucesso!
pause
