![Medpr](https://user-images.githubusercontent.com/68227124/215276510-905c580f-1b2e-4247-826d-e365bee0fd86.jpg)

# Hello there!
This repo was originally made for IT-Academy's ASP.NET Enterprise Development course. Today it's a size of a decent pet-project.

	As illustrated  above, Medpr has 3 parts:
		🙂 Angular app
		🧠 ASP.NET Core WebAPI
		💾 SQL Server
Docker compose is configured to build 3 linux-based containers, seed data to SQL server and bind mounts to Angular app.

Here is an example of seeded users:

	👉 Admin user: admin@admin.com | Admin_1_Admin
	👉 Main user: firstuser@gmail.com | 8yQD!yya

Check out the very first UI mockup: https://xd.adobe.com/view/c5b3da8e-d86b-435c-88ad-82eb5d7a315a-90e0/

  ## Current plans
  1. Add xUnit & MOQ tests (✔️)
  2. Integrate docker (✔️)
  
  	2.1. Dockerfile for WebAPI (✔️)
  	2.2. Containerize SQL Server & seed data (✔️)
  	2.3. Dockerfile for Angular with bind-mounts to recompile on change (✔️)
  	2.4. Docker-compose (✔️)
	
  3. GitHub Actions (✔️)
  
 	3.1. Build & test .NET (✔️)
  	3.2. CI to Docker Hub (✔️)
  	3.3. CodeQL (✔️)

  ## MVC project history
  1. password -> password hash migration (✔️)
  2. basic MVC stracture (✔️)
  3. CRUD for 1 db branch (✔️)
  4. CRUD for all endpoint branches (✔️)
  5. [ValidateAntiForgeryToken] to post methods
  6. Add Serilog (✔️)
  7. Add validation tags (✔️)
  8. UnitOfWork architecture (✔️)
  9. Delete confirmation page (✔️)
  10. CRUD for table with select (✔️)
  11. CRUD for all tables with select (✔️)
  12. CRUD for all tables (✔️)
  13. Layout for every table (🖐) 
  14. Date validation on input (✔️)
  15. Add simultaneous remote checking for 
	beginning and ending dates on presctiption CRUD.
	As for now validation works, but sticks to the field
	last modified (🖐 ➡ solved by reactive forms in 3.2) 
  16. Add authentification and authorization (✔️)
  17. Adjust views and controllers with authentification logic (✔️)
  18. Add redirect to error page if soemthing goes wrong (✔️)
  19. User profile feature (🖐) 
  20. Find and add to surnames functionality (✔️)
  ## Web API project history
  1. WebAPI CRUD status codes unification (✔️)
  2. HATEOAS link generator as extension method (✔️)
  3. Host name configuration (🖐) 
  4. Rebuild services on CQS base for WebAPI (✔️)
  5. Notifications with Hangfire for appointments, prescriptions and vaccinations (✔️)
  6. Generate drug from external WebAPI (✔️)
  7. Authentication and authorization for WebAPI using JWT (✔️)
  8. Reimplement MVC app fucntionality for WebAPI project (✔️)
  9. Notifications Hangfire part (✔️)
  10. Notifications with SignalR for appointments, prescriptions and vaccinations (✔️)
  ## Angular project history
  1. Angular Material (✔️)
  2. Reactive forms (✔️)
  3. Spinner & errors (✔️)
  4. Routing (✔️)
  5. Consistent layout (✔️ ⬅ 1.13)
  6. Redux/NgRx store (✔️)
  7. Authentication guard (✔️)
  8. Interceptors adding JWT to requests (✔️)
  9. Notifications toastr (✔️)
  10. SignalR notifications (✔️ ⬅ 2.10)
  11. Scale Angular client to every entity provided (✔️)
  12. User profile feature (✔️ ⬅ 1.19)
  13. Family basic search (✔️)
  14. Family manager (✔️)
  15. User feed (✔️)
  16. Simple animation (✔️)
