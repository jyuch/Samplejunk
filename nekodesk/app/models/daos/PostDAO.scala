package models.daos

import java.util.UUID

import models.Post

import scala.concurrent.Future

trait PostDAO {
  def save(post: Post): Future[Unit]

  def list: Future[Seq[Post]]

  def findById(id:UUID):Future[Option[Post]]

  def deleteById(id:UUID):Future[Unit]
}
