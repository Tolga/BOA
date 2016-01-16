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

        public static string GetCommitersList()
        {
            return @"p: Project = input;
                    project: output collection[string] of string;
                    committers: map[string] of bool;

                    visit(p, visitor {
	                    before node: Revision ->
		                    if (!haskey(committers, node.committer.username)) {
			                    committers[node.committer.username] = true;
                                project[p.id] << node.committer.username;
		                    }
                    });";
        }

        public static string GetCommitedJavaFiles()
        {
            return @"p: Project = input;
                    project: output collection[string] of string;
                    commit_date: time;

                    visit(p, visitor {
                        before node: Revision -> commit_info = format(""%s = %s = %s = %s"", node.id, node.committer.username, node.commit_date);
                        before node: ChangedFile -> project[p.id] << format(""%s = %s"", commit_info, node.change);
                    });";
        }
    }
}
