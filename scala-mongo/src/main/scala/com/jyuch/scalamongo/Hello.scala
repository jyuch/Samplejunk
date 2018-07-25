package com.jyuch.scalamongo

import org.mongodb.scala.bson.collection.immutable.Document
import org.mongodb.scala.{MongoClient, MongoDatabase}

import scala.concurrent.Await
import scala.language.postfixOps
import scala.concurrent.duration._

object Hello extends App {

  val mongo = MongoClient("mongodb://192.168.0.100:27017")
  val db = mongo.getDatabase("hogefuga")
  val collection = db.getCollection("piyo")

  // make a document and insert it
  val doc: Document = Document("_id" -> 1, "name" -> "MongoDB", "type" -> "database",
    "count" -> 1, "info" -> Document("x" -> 203, "y" -> 102))

  val a = collection.insertOne(doc)

  Await.result(a.toFuture(), 10 seconds)

  mongo.close()
}
