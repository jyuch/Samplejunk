package models.daos

import java.util.UUID

import models.Post

import scala.collection.mutable
import scala.concurrent.Future

class PostDAOImpl extends PostDAO {

  import PostDAOImpl.posts

  override def save(post: Post) = {
    posts += (post.id -> post)
    Future.successful(())
  }

  override def list = {
    Future.successful(posts.map(_._2).toSeq)
  }

  override def findById(id: UUID) = {
    Future.successful(posts.get(id))
  }

  override def deleteById(id: UUID) = {
    posts -= id
    Future.successful(())
  }
}

object PostDAOImpl {
  val posts: mutable.HashMap[UUID, Post] = mutable.HashMap()
}