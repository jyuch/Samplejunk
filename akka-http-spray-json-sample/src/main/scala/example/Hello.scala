package example

import akka.actor.ActorSystem
import akka.http.scaladsl.Http
import akka.http.scaladsl.marshallers.sprayjson.SprayJsonSupport
import akka.http.scaladsl.model._
import akka.http.scaladsl.server.Directives._
import akka.stream.ActorMaterializer
import example.Messages.{Message, User}
import spray.json.DefaultJsonProtocol

import scala.io.StdIn

object Messages {

  case class User(name: String, address: String)

  case class Message(to: List[User], subject: String, body: String)

}

object MessagesJsonProtocol extends DefaultJsonProtocol {

  import Messages._

  implicit lazy val userFormat = jsonFormat2(User)
  implicit lazy val messageFormat = jsonFormat3(Message)
}

object Hello extends App with SprayJsonSupport {
  implicit val system = ActorSystem("my-system")
  implicit val materializer = ActorMaterializer()
  implicit val executionContext = system.dispatcher

  import MessagesJsonProtocol._

  val route =
    path("hello") {
      get {
        complete(HttpEntity(ContentTypes.`text/html(UTF-8)`, "<h1>Say hello to akka-http</h1>"))
      }
    } ~ path("user") {
      post {
        entity(as[User]) { user =>
          complete(s"Hello ${user.name} ${user.address}")
        }
      }
    } ~ path("message") {
      post {
        entity(as[Message]) { message =>
          complete(message)
        }
      }
    }

  val bindingFuture = Http().bindAndHandle(route, "localhost", 8080)

  println(s"Server online at http://localhost:8080/\nPress RETURN to stop...")
  StdIn.readLine() // let it run until user presses return
  bindingFuture
    .flatMap(_.unbind()) // trigger unbinding from the port
    .onComplete(_ => system.terminate()) // and shutdown when done
}