## General ##
Le but de ce kata est de voir comment vous abordez un problème particulier dans un contexte réel.

Veuillez considérer votre travail comme du code qui sera prêt pour faire la mise en production.

Une fois que vous avez terminé, merci de créer une Pull Request sur votre feature branche vers la branche `main` à fin que les membres de l'équipe puissent vous faire des commentaires.

## Objectifs ##
Dans le cadre de ce projet, nous souhaitons avoir une Api avec les spécifications suivantes:
1. Je veux que mon sensor récupère la température provenant du composant `TemperatureCaptor` (renvoi la température en °C);
2. Je veux que l'état de mon Sensor soit à "HOT" lorsque la température captée est suppérieure ou égale a 40°C;
3. Je veux l'état de mon Sensor soit à "COLD" lorsque la température captée est inferieur a 22°C;
4. Je veux l'état de mon Sensor soit à "WARM" lorsque la température captée est entre 22°C et inferieur à 40°C;
5. Je veux récuperer l'historique des 15 dernieres demandes des températures;
6. Je veux pouvoir redefinir les limites pour "HOT", "COLD", "WARM".

## Stack Technique ##
- .Net Core 6.0 ou supérieur.
- SQL Lite.
- Docker.
