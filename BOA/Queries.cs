namespace BOA
{
    public class Queries
    {
        public static string GetDevelopersList()
        {
            return @"p: Project = input;
                    counts: output collection[string] of string;

                    foreach (i: int; def(p.developers[i]))
                        counts[p.id] << format(""%s - %s"", p.developers[i].username, p.developers[i].email);";
        }

        public static string GetProjectsPerDeveloper()
        {
            return @"p: Project = input;
                    counts: output top(1000) of string weight int;

                    foreach (i: int; def(p.developers[i]))
	                    counts << p.developers[i].username weight 1;";
        }

        public static string GetAuthorsList()
        {
            return @"p: Project = input;
                    project: output collection[string] of string;
                    committers: map[string] of bool;

                    visit(p, visitor {
	                    before node: Revision ->
		                    if (!haskey(committers, node.author.username)) {
			                    committers[node.author.username] = true;
                                project[p.id] << node.author.username;
		                    }
                    });";
        }

        public static string FileChangesPerCommit(int top = 10)
        {
            return @"
            p: Project = input;
            counts: output top(" + top + @") of string weight int;
            commit_info:= """";
            changes:= """";
            added:= 0;
            modified:= 0;
            deleted:= 0;

            visit(p, visitor {
                before node: Revision->commit_info = format(""%s # %s # %s # %s"", p.id, node.id, node.committer.username, node.commit_date);
                before node: ChangedFile->  {
                    if (node.change == ChangeKind.ADDED)
                        added++;
                    if (node.change == ChangeKind.MODIFIED)
                        modified++;
                    if (node.change == ChangeKind.DELETED)
                        deleted++;
                }
                before _ -> {
                    changes = format(""%s # %s # %s"", added, modified, deleted);
                    counts << format(""%s | %s"", commit_info, changes) weight 1;
                    added = 0;
                    modified = 0;
                    deleted = 0;
                }
            });";
        }

        public static string Commits(int top = 5000000)
        {
            return @"p: Project = input;
                        tempString := """";
                        files: output top(" + top + @") of string weight int;

                        visit(p, visitor {
	                        before node: Revision -> {
	                            tempString = format(""%s # %s # %s # %s"", p.id, node.id, node.author.username, node.commit_date);
	                        }
	                        before node: ChangedFile -> {
			                    if (node.change == ChangeKind.ADDED)
			                        files << format(""%s # %s"", tempString, ""A"") weight 1;
			                    if (node.change == ChangeKind.DELETED)
			                        files << format(""%s # %s"", tempString, ""D"") weight 1;
			                    if (node.change == ChangeKind.MODIFIED)
			                        files << format(""%s # %s"", tempString, ""M"") weight 1;
	                        }
                    });";
        }
    }
}
