#
# Build stage
#
FROM maven:3.6.3-openjdk-17 AS build
COPY src /home/app/src
COPY pom.xml /home/app
RUN mvn -f /home/app/pom.xml clean package

#
# Package stage
#
FROM openjdk:17
COPY --from=build /home/app/target/ /usr/local/lib/
EXPOSE 8080
ENTRYPOINT ["java","-jar","/usr/local/lib/student-manager-bot-1.0-SNAPSHOT.jar"]
