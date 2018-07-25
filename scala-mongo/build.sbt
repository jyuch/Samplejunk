import Dependencies._

lazy val akkaHttpVersion = "10.0.11"
lazy val akkaVersion    = "2.5.8"

lazy val root = (project in file(".")).
  settings(
    inThisBuild(List(
      organization := "com.jyuch",
      scalaVersion := "2.12.4",
      version := "0.1.0-SNAPSHOT"
    )),
    name := "scala-mongo",
    libraryDependencies ++= Seq(
      "com.typesafe.akka" %% "akka-http"            % akkaHttpVersion,
      "com.typesafe.akka" %% "akka-http-spray-json" % akkaHttpVersion,
      "com.typesafe.akka" %% "akka-http-xml"        % akkaHttpVersion,
      "com.typesafe.akka" %% "akka-stream"          % akkaVersion,
      "org.mongodb.scala" %% "mongo-scala-driver"   % "2.2.1"
    )
  )
