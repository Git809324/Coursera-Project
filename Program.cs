var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();
